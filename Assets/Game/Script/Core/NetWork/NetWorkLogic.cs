#define WMSJ_PROFILER
using UnityEngine;
using System.Net;
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace Core.NetWork
{
    public class NetworkLogic
    {
        private const int PACKET_MESSAGEID_SIZE = sizeof(UInt16);   // 消息包包头部分大小
        private const int PACKET_LEN_SIZE = sizeof(Int32);          // 消息包长度部分大小
        private const int PACKET_SIZE_MAX = 1024 * 256;             // 单个消息包最大长度
        private const int STREAM_SIZE_MAX = 1024 * 512;             // 字节流最大长度，超出此长度断开连接，清空流
        private const int DEFAULT_PROCESSPACKET_COUNT = 12;
        private const int PACKET_CRC_SIZE = sizeof(UInt32);
        private const int PACKET_SEQ_SIZE = sizeof(UInt32);
        private static INetPlatformAdapter _netPlatformAdapter;
        private static IPacketDispatcher _packetDispatcher;
        // 单件类
        private NetworkLogic()
        {
        }
        
    public static void SetAdapter(IPacketDispatcher packetDispatcher,INetPlatformAdapter adapter)
        {
            _packetDispatcher = packetDispatcher;
            _netPlatformAdapter = adapter;
        }

        // 基础数据
        private static SocketAPI m_socket = new SocketAPI();
        private static MemoryStream m_msInput = new MemoryStream();        // 输入流缓存
        private static MemoryStream m_msOutput = new MemoryStream();       // 输出流缓存
        private static byte[] m_byteReceiveCache = new byte[PACKET_SIZE_MAX];   // receive数组
        private static byte[] m_bytePacketIDCache = new byte[PACKET_MESSAGEID_SIZE];   //packetID缓存
        private static byte[] m_bytePacketLenCache = new byte[PACKET_LEN_SIZE];         //packet长度缓存
        private static byte[] m_bytePacketProtobufCache = new byte[1024];              //protobuf缓存
        private static MemoryStream m_msPacketCache = new MemoryStream();
        private static bool m_bCanReceivePacket = true;
        private static int m_processPacketCountEachFrame = DEFAULT_PROCESSPACKET_COUNT;
        private static int m_protobufPacketCacheLen = 1024;                     //protobuf缓存长度（自增长）
        //private static byte[] m_bytePacketCrcCache = new byte[PACKET_CRC_SIZE];
        //private static byte[] m_bytePacketSeqCache = new byte[PACKET_SEQ_SIZE];

        private static UInt32 m_ClientSeq = 0;
        private static bool m_CanSendPacket = false;
        public static byte[] Session = new byte[] { };
        private static byte[] remainByte = new byte[1024]; // 
        // 状态相关
        public enum ConnectState
        {
            DISCONNECT,         // 未连接
            CONNECTING,         // 连接中
            CONNECTED,          // 已连接
        }

        public delegate void ConnectChangeDelegate(ConnectState newState);
        public static ConnectState State { get { return m_connectState; } }
        private static ConnectState m_connectState = ConnectState.DISCONNECT;
        private static ConnectChangeDelegate m_delConnectChange;

        private static bool m_bGetConnectResult = false;
        private static bool m_bConnectSuccess = false;

        public static void SetCanSendPacket(bool value)
        {
            m_CanSendPacket = value;
        }

        public static void SetStateListener(ConnectChangeDelegate delListener)
        {
            m_delConnectChange = delListener;
        }

        public static void StopReceivePacket()
        {
            m_bCanReceivePacket = false;
        }

        public static void StartReceivePacket()
        {
            m_bCanReceivePacket = true;
        }

        private static void ChangeConnectState(ConnectState newState)
        {
            ConnectState lastState = m_connectState;
            m_connectState = newState;
            if (null != m_delConnectChange && lastState != newState && lastState != ConnectState.CONNECTING)
            {
                m_delConnectChange(newState);
            }
        }

        //输出通信消息开关_方便定位消息包
        private static bool m_bLOgPacketInfoLogOpen = false;
        public static void MarkLogPacetInfoOpen(bool bFlag)
        {
            m_bLOgPacketInfoLogOpen = bFlag;
        }

        private static void OnSendBytes(int nBytes)
        {
            m_nSendDataLenth += nBytes;
            m_nTotalSendDataLenth += (ulong)(nBytes);
        }

        private static void OnRecvBytes(int nBytes)
        {
            m_nRecvDataLenth += nBytes;
            m_nTotalRecvDataLenth += (ulong)(nBytes);
        }

        //每秒发送/接收量
        private static int m_nSendDataLenth = 0;
        private static int m_nRecvDataLenth = 0;

        //总发送/接收量
        private static ulong m_nTotalSendDataLenth = 0;
        private static ulong m_nTotalRecvDataLenth = 0;

        public static int NowRecvBytes
        {
            get { return m_nRecvDataLenth; }
            set { m_nRecvDataLenth = value; }
        }
        public static int NowSendBytes
        {
            get { return m_nSendDataLenth; }
            set { m_nSendDataLenth = value; }
        }

        public static ulong TotalRecvBytes { get { return m_nTotalRecvDataLenth; } }
        public static ulong TotalSendBytes { get { return m_nTotalSendDataLenth; } }

        public static void SendPacket(ProtoPacket packet)
        {
            if (m_connectState != ConnectState.CONNECTED)
            {
                return;
            }

            if (false == m_CanSendPacket)
            {
                return;
            }

            byte[] packetArray = packet.GetPacketArray();

            // packet length
            int curMsgLen = packetArray.Length + PACKET_MESSAGEID_SIZE + PACKET_LEN_SIZE + PACKET_CRC_SIZE + PACKET_SEQ_SIZE;
            byte[] messagelenbyte = System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(curMsgLen));
            m_msOutput.Write(messagelenbyte, 0, messagelenbyte.Length);

            // messageid
            Int16 curMsgID = packet.GetMessageID();
            byte[] messageidbyte = System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(curMsgID));
            m_msOutput.Write(messageidbyte, 0, messageidbyte.Length);

            // seq
            byte[] seqByte = System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((int)m_ClientSeq++));
            m_msOutput.Write(seqByte, 0, seqByte.Length);

            // xor
            if (_packetDispatcher.IsCryptoPacket(curMsgID) && null != Session)
            {
                SendXorCrypto.XorEncrypt(packetArray, (uint)packetArray.Length, Session);
            }

            // crc
            byte[] crc = System.BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder((int)CRC32.GetCRC32(packetArray)));
            m_msOutput.Write(crc, 0, crc.Length);

            // data
            m_msOutput.Write(packetArray, 0, (int)packetArray.Length);

            // log
            if (true == m_bLOgPacketInfoLogOpen)
            {
                //Debug.Log("Send Packet: Id[" + curMsgID.ToString() + "] Name[" + packet.GetType() + "]");
            }
        }

        // 输出流处理
        private static void ProcessOutput()
        {
            long nByteNeedToSend = m_msOutput.Length;
            // 取出要发送的数据
            if (nByteNeedToSend == 0)
            {
                return;
            }
            if (nByteNeedToSend > STREAM_SIZE_MAX)
            {
                // 如果输入流大小大于定义的大小，断开连接
                Debug.LogError("send error send stream out of range: cur-" + nByteNeedToSend.ToString() + " max-" + STREAM_SIZE_MAX);
                ConnectLost();
                return;
            }
            byte[] sendBytes = m_msOutput.GetBuffer();

            int sendByteOffset = 0;
            int leftBytes = (int)nByteNeedToSend;
            while (leftBytes > 0)
            {
                int retSend = m_socket.Send(sendBytes, sendByteOffset, leftBytes);
                if (retSend < 0)
                {
                    ConnectLost();
                    return;
                }
                else if (retSend == 0)
                {
                    ConnectLost();
                    return;
                }
                else
                {
                    OnSendBytes(retSend);

                    leftBytes -= retSend;
                    sendByteOffset += retSend;
                }
            }
            // 清空输出流
            m_msOutput.Position = 0;
            m_msOutput.SetLength(0);
        }

        // 输入流处理
        private static void ProcessInput()
        {
            if (!m_socket.PollRead())
            {
                return;
            }

            int recvCount = 0;

            recvCount = m_socket.Recv(m_byteReceiveCache, PACKET_SIZE_MAX);
            if (recvCount < 0)
            {
                ConnectLost();
                return;
            }
            else if (recvCount == 0)
            {
                ConnectLost();
                return;
            }

            if (recvCount > 0)
            {
                OnRecvBytes(recvCount);

                m_msInput.Write(m_byteReceiveCache, 0, (int)recvCount);
            }
        }

        private static void ProcessPacket()
        {
            /*if(PlatformHelper.IsEnableMinMemMode(PlatformHelper.EnableFuncMinMem.MemoryLog))
            {
                AndroidMemoryProfiler.Sample("NetworkLogic.ProcessPacket Start");
            }*/
            int processPacketCountMax = m_processPacketCountEachFrame;
            m_msInput.Position = 0;
            while (m_bCanReceivePacket && processPacketCountMax-- > 0)
            {
                int curStreamLength = (int)(m_msInput.Length - m_msInput.Position);
                if (curStreamLength < PACKET_LEN_SIZE)
                {
                    //接收到的数据消息长度头大小，等下次
                    break;
                }

                // 读取消息包长度
                m_msInput.Read(m_bytePacketLenCache, 0, PACKET_LEN_SIZE);
                int packetLength = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(m_bytePacketLenCache, 0));

                if (curStreamLength < packetLength)
                {
                    // 接收到的数据消息整个包体长度，继续接收
                    m_msInput.Position -= PACKET_LEN_SIZE;
                    break;
                }

                // 算出protobuf大小
                int probufLength = packetLength - PACKET_LEN_SIZE - PACKET_MESSAGEID_SIZE;
                m_msInput.Read(m_bytePacketIDCache, 0, PACKET_MESSAGEID_SIZE);
                short messageid = BitConverter.ToInt16(m_bytePacketIDCache, 0);
                messageid = System.Net.IPAddress.NetworkToHostOrder(messageid);

                m_msPacketCache.Position = 0;
                m_msPacketCache.SetLength(0);
                {
                    if (probufLength > m_protobufPacketCacheLen)
                    {
                        m_bytePacketProtobufCache = new byte[probufLength];
                        m_protobufPacketCacheLen = probufLength;
                    }
                    m_msInput.Read(m_bytePacketProtobufCache, 0, probufLength);

                    if (_packetDispatcher.IsCryptoPacket(messageid))
                    {
                        ReceiveXorCrypto.XorDecrypt(m_bytePacketProtobufCache, (uint)probufLength);
                    }

                    m_msPacketCache.Write(m_bytePacketProtobufCache, 0, probufLength);
                    m_msPacketCache.Position = 0;
                }
                try
                {
                    /*if (DebugInfo.EnableProfilerSample)
                    {
                        CustomSampler mySampler = PTProfilerSampler.GetPacketSampler(messageid);
                        mySampler.Begin();
                    }*/
                    
                    #if WMSJ_PROFILER
                    UnityEngine.Profiling.Profiler.BeginSample("ReceivePacket" + messageid);
#endif
                    /*if(!DebugInfo.EnablePacketDeal)
                    {
                        //                         if(messageid == (short)MessageID.PACKETID_XX_RESPONSE_HEARTBEAT
                        //                             || messageid == (short)MessageID.PACKETID_XX_REQUEST_HEARTBEAT 
                        //                             )
                       if(messageid != (short)MessageID.PACKETID_GC_BUFF_SYNC_INFO) 
                        {
                            PacketDispatcher.ReceivePacket(messageid, m_msPacketCache);
                        }
                    }
                    else
                    {
                        DeviceBehaviorTrack.ReceivePacketBegin(messageid);*/
                    _packetDispatcher.ReceivePacket(messageid, m_msPacketCache);

                        /*DeviceBehaviorTrack.ReceivePacketEnd(messageid);
                    }*/

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    m_msPacketCache.Position = 0;
                //    ReplayLogic.Record(messageid, m_msPacketCache);
#endif

#if WMSJ_PROFILER
                    UnityEngine.Profiling.Profiler.EndSample();
#endif
                    /*if (DebugInfo.EnableProfilerSample)
                    {
                        CustomSampler mySampler = PTProfilerSampler.GetPacketSampler(messageid);
                        mySampler.End();

                        PTProfilerSampler.RecordProfiler(mySampler);
                    }*/

                    // 调试日志_方便定位消息协议
                    if (true == m_bLOgPacketInfoLogOpen)
                    {
                        //string packetName = PacketDispatcher.PacketName(messageid);
                        //Debug.Log("Recv Packet: Id[" + messageid.ToString() + "] Name[" + packetName + "]");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(string.Format("Exception messageid:{0}, msg:{1} \r\n stackTrace: {2}", messageid, e.Message,e.StackTrace));
                }
            }

            if (processPacketCountMax >= 0)
            {
                m_processPacketCountEachFrame = DEFAULT_PROCESSPACKET_COUNT;
            }
            else
            {
                m_processPacketCountEachFrame += 4;
            }

            //剩余的写入流中,下一帧解析
            int remain = (int)(m_msInput.Length - m_msInput.Position);
            if (remain > 0)
            {
                if (remain > remainByte.Length)
                {
                    remainByte = new byte[remain];
                }
                m_msInput.Read(remainByte, 0, remain);
                m_msInput.Position = 0;
                m_msInput.SetLength(0);
                m_msInput.Write(remainByte, 0, remain);
            }
            else
            {
                m_msInput.Position = 0;
                m_msInput.SetLength(0);
            }
            /*if (PlatformHelper.IsEnableMinMemMode(PlatformHelper.EnableFuncMinMem.MemoryLog))
            {
                AndroidMemoryProfiler.Sample("NetworkLogic.ProcessPacket End");
            }*/
        }


        // 连接服务器
        public delegate void DelConnectResult(bool bSuccess);
        static Thread m_hConnectThread = null;
        static string m_strConnectIP;
        static int m_nConnectPort;
        static DelConnectResult m_delConnectResult;
        public static void Connect(string strIP, int port, DelConnectResult delConnectResult)
        {
            if (m_connectState == ConnectState.CONNECTING)
            {
                Debug.Log("alread connecting");
                return;
            }
            m_strConnectIP = strIP;
            m_nConnectPort = port;
            m_delConnectResult = delConnectResult;
            m_ClientSeq = 0;
            SetCanSendPacket(false);
            Close();
            ChangeConnectState(ConnectState.CONNECTING);

            m_hConnectThread = new Thread(new ThreadStart(ConnectThread));
            m_hConnectThread.Start();
        }

        // 连接线程
        public static void ConnectThread()
        {
            string realConnectIP = _netPlatformAdapter.TryConvIP(m_strConnectIP);
            string result = m_socket.Connect(realConnectIP, m_nConnectPort);
            if (string.IsNullOrEmpty(result))
            {
                m_bConnectSuccess = true;
            }
            else
            {
                Debug.Log("socket connect result:" + result);
                m_bConnectSuccess = false;
            }

            m_bGetConnectResult = true;
        }

        //主动断开连接
        public static void Disconnect()
        {
            Debug.Log("network disconect");
            Close();
            m_connectState = ConnectState.DISCONNECT;
        }
        // 出错后被动断开连接
        public static void ConnectLost()
        {
            Debug.Log("network connect lost");
            Close();
            ChangeConnectState(ConnectState.DISCONNECT);
        }

        // 关闭连接并清空流
        private static void Close()
        {
            m_socket.Close();
            m_msInput.Position = 0;
            m_msInput.SetLength(0);
            m_msOutput.Position = 0;
            m_msOutput.SetLength(0);
        }

        // 心跳
        public static void Update()
        {
            if (m_bGetConnectResult)
            {
                m_bGetConnectResult = false;
                ChangeConnectState(m_bConnectSuccess ? ConnectState.CONNECTED : ConnectState.DISCONNECT);
                if (true != m_bConnectSuccess)
                {
                    DelConnectResult delConnect = m_delConnectResult;
                    m_delConnectResult = null;
                    if (null != delConnect) delConnect(m_bConnectSuccess);
                }
            }
            if (m_connectState != ConnectState.CONNECTED)
            {
                return;
            }
            #if WMSJ_PROFILER
            UnityEngine.Profiling.Profiler.BeginSample("ProcessOutput");
#endif
            ProcessOutput();
            #if WMSJ_PROFILER
            UnityEngine.Profiling.Profiler.EndSample();
#endif
#if WMSJ_PROFILER
            UnityEngine.Profiling.Profiler.BeginSample("ProcessInput");
#endif
            ProcessInput();
#if WMSJ_PROFILER
            UnityEngine.Profiling.Profiler.EndSample();
#endif
#if WMSJ_PROFILER
            UnityEngine.Profiling.Profiler.BeginSample("ProcessPacket");
#endif
            ProcessPacket();
#if WMSJ_PROFILER
            UnityEngine.Profiling.Profiler.EndSample();
#endif
#if WMSJ_PROFILER
            UnityEngine.Profiling.Profiler.BeginSample("ProcessOutput");
#endif
            ProcessOutput();
#if WMSJ_PROFILER
            UnityEngine.Profiling.Profiler.EndSample();
#endif
        }

        public static void ExecConnectCB()
        {
            DelConnectResult delConnect = m_delConnectResult;
            m_delConnectResult = null;
            if (null != delConnect) delConnect(m_bConnectSuccess);
        }

        public static string ParseIPAddress(string resource)
        {
            string result = resource;
            do
            {
                IPAddress rAddress;
                if (IPAddress.TryParse(resource, out rAddress))
                {
                    break;
                }

                List<string> ipv4List = new List<string>();
                List<string> ipv6List = new List<string>();
                IPAddress[] ipAddrList = null;
                try
                {
                    ipAddrList = Dns.GetHostAddresses(resource);
                }
                catch
                {
                }
                if (null == ipAddrList || ipAddrList.Length <= 0)
                {
                    break;
                }

                for (int tmpi = 0; tmpi < ipAddrList.Length; ++tmpi)
                {
                    if (ipAddrList[tmpi].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        ipv6List.Add(ipAddrList[tmpi].ToString());
                    }
                    if (ipAddrList[tmpi].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipv4List.Add(ipAddrList[tmpi].ToString());
                    }
                }

                result = (ipv6List.Count == 0) 
                    ? ipv4List[UnityEngine.Random.Range(0, ipv4List.Count)] 
                    : ipv6List[UnityEngine.Random.Range(0, ipv6List.Count)];
            } while (false);
            Debug.Log(string.Format("parseipaddress resource{0} target{1}", resource, result));
            return result;
        }
    }
}

