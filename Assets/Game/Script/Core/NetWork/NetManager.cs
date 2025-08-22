using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Base;
using Core.EventBus;
using UnityEngine;

namespace Core.NetWork
{
    internal struct ConnectChangeEventMessage : IEventMessage
    {
        public NetworkLogic.ConnectState ChangeType;
    }

    public class NetManager : ModuleManager<NetManager>
    {
        public static bool IsReconnecting = false;

        private string m_connectIP;
        private int m_connectPort;
        private string m_reconnectIP;
        private string m_reconnectIP2;
        private int m_reconnectPort;

        private NetworkLogic.DelConnectResult m_delConnect;

        /// <summary>
        ///     解析用的handlers
        /// </summary>
        private static readonly Dictionary<int, PacketHandler> _DesHandlers = new();

        /// <summary>
        ///     处理消息包用的handlers
        /// </summary>
        private static readonly Dictionary<Type, object> _packetHandlers = new();


        public override void Start()
        {
            NetworkLogic.SetStateListener(OnConnectStateChanged);
            RegisterDesHandlers();
        }

        public override void Tick(float deltaTime)
        {
            // GCPlayerList.Clear();
            NetworkLogic.Update();
        }

        public override void End()
        {
            base.End();
            _DesHandlers.Clear();
        }

        #region 协议注册回调

        public static void RegisterDesHandlers()
        {
            var handlerType = typeof(PacketHandler);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => handlerType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in types)
            {
                var handler = (PacketHandler)Activator.CreateInstance(type);
                _DesHandlers[handler.GetMessageId()] = handler;
            }
        }

        public bool ReceivePacket(short messageId, Stream packetData)
        {
            if (_DesHandlers.TryGetValue(messageId, out var handler))
            {
                handler.ReceivePacket(messageId, packetData);
                return true;
            }

            return false;
        }

        public bool RegisterPacketHandler<T>(Action<T> handler) where T : IGCPacket
        {
            if (!_packetHandlers.ContainsKey(typeof(T)))
            {
                _packetHandlers[typeof(T)] = handler;
                return true;
            }

            Debug.LogError($"失败，已经注册过了 {typeof(T).Name}");
            return false;
        }

        public void UnRegisterPacketHandler<T>() where T : IGCPacket
        {
            UnRegisterPacketHandler(typeof(T));
        }

        public void UnRegisterPacketHandler(Type packetHandler)
        {
            if (_packetHandlers.ContainsKey(packetHandler)) _packetHandlers.Remove(packetHandler);
        }

        public void BroadcastPacket<T>(T packet) where T : IGCPacket
        {
            if (_packetHandlers.TryGetValue(typeof(T), out var obj) && obj is Action<T> handler)
                handler.Invoke(packet);
            else
                Debug.LogError($"消息转发失败，没有注册 {typeof(T).Name}");
        }

        #endregion

        public void SetPlatformAdapter(IPacketDispatcher packetDispatcher, INetPlatformAdapter adapter)
        {
            NetworkLogic.SetAdapter(packetDispatcher, adapter);
        }

        public void ReconnectToBigWorld(string ip, int port, NetworkLogic.DelConnectResult delConnect)
        {
            EventManager.Instance.RaiseEvent(new ConnectChangeEventMessage
                { ChangeType = NetworkLogic.ConnectState.DISCONNECT });

            //GameManager.OnConnectLost();
            var address = NetworkLogic.ParseIPAddress(ip);
            ConnectToServer(address, port, false, delConnect);
        }

        public void ReconnectToBattleWorld(string ip, int port, NetworkLogic.DelConnectResult delConnect)
        {
            EventManager.Instance.RaiseEvent(new ConnectChangeEventMessage
                { ChangeType = NetworkLogic.ConnectState.DISCONNECT });

            // GameManager.OnConnectLost();
            ConnectToServer(ip, port, false, delConnect);
        }

        public void ReconnectToServer(NetworkLogic.DelConnectResult delConnect)
        {
            EventManager.Instance.RaiseEvent(new ConnectChangeEventMessage
                { ChangeType = NetworkLogic.ConnectState.DISCONNECT });

            //GameManager.OnConnectLost();
            ConnectToServer(m_reconnectIP, m_reconnectPort, false, delConnect);
        }

        public void ReconnectToServerWithIP2(NetworkLogic.DelConnectResult delConnect)
        {
            // GameManager.OnConnectLost();

            if (string.IsNullOrEmpty(m_reconnectIP2))
            {
                delConnect(false);
                return;
            }

            Debug.Log("connecting ip2:" + m_reconnectIP2 + " port:" + m_reconnectPort);
            ConnectToServer(m_reconnectIP2, m_reconnectPort, false, delConnect);
        }

        public void ConnectToServerWithDualIP(string _ip, string ip2, int _port, bool bUseReconnect,
            NetworkLogic.DelConnectResult delConnect)
        {
            if (bUseReconnect) m_reconnectIP2 = ip2;

            ConnectToServer(_ip, _port, bUseReconnect, delConnect);
        }

        public void ConnectToServer(string _ip, int _port, bool bUseReconnect, NetworkLogic.DelConnectResult delConnect)
        {
            m_connectIP = _ip;
            m_connectPort = _port;
            if (bUseReconnect)
            {
                m_reconnectIP = _ip;
                m_reconnectPort = _port;
            }

            m_delConnect = delConnect;
            DoConnectToServer();
        }

        /*private void OnConnectCheckResVersion(UpdateHelper.CheckVersionResult result)
        {
            if (result == UpdateHelper.CheckVersionResult.NEEDUPDATE)
            {
                // 需要资源更新，退出游戏重新登录
                CloseReconnectTipAndShowQuitBox();
            }
            else if (result == UpdateHelper.CheckVersionResult.NONEEDUPDATE)
            {
                DoConnectToServer();
            }
            else
            {
                NetworkLogic.DelConnectResult delTmp = m_delConnect;
                m_delConnect = null;
                if (null != delTmp)
                    delTmp(false);
            }
        }*/

#if ZEUS_HOTHIX
    private void HandleHotfixCheckError(Zeus.Framework.Hotfix.HotfixError error)
    {
        //把错误都放到启动时的热更流程里处理，简化逻辑
        CloseReconnectTipAndShowQuitBox();
    }

    private void HandleHotfixCheckSucceed(Zeus.Framework.Hotfix.HotFixType type, double size, bool ispredownload)
    {
        switch(type)
        {
            case Zeus.Framework.Hotfix.HotFixType.None:
            case Zeus.Framework.Hotfix.HotFixType.Recommend:
                //推荐更新不退出游戏,等下次登录再更新
                StartCoroutine(TagUpdateHelper.CheckResVersion(OnConnectCheckResVersion));
                break;
            case Zeus.Framework.Hotfix.HotFixType.Force:
            case Zeus.Framework.Hotfix.HotFixType.AppStore:
                //预下载不需要退出游戏进行更新
                if(!ispredownload)
                {
                    //有更新的话，全部放到启动时的热更流程里处理，简化逻辑
                    CloseReconnectTipAndShowQuitBox();
                }
                else
                {
                    StartCoroutine(TagUpdateHelper.CheckResVersion(OnConnectCheckResVersion));
                }
                break;
        }
    }
#endif

        private void CloseReconnectTipAndShowQuitBox()
        {
            /*if (LoginController.Instance() != null)
            {
                if (ReconnectTipController.Instance() != null)
                {
                    ReconnectTipController.Instance().OnNeedLogin();
                    UIManager.CloseUI(UIInfo.ReconnectTip);
                }

                MessageBoxController.OpenOKBox(6, 2, OnClickQuitGame);
            }*/
        }

        //连接上次连接过的服务器
        private void DoConnectToServer()
        {
            var delTmp = m_delConnect;
            m_delConnect = null;
            NetworkLogic.Connect(m_connectIP, m_connectPort, delTmp);
        }

        //网络环境变化
        private void OnConnectStateChanged(NetworkLogic.ConnectState state)
        {
            EventManager.Instance.RaiseEvent(new ConnectChangeEventMessage { ChangeType = state });
        }

        private void OnClickQuitGame()
        {
            //   GameManager.QuitGame();
        }

        public void SetCanSendPacket(bool value)
        {
            NetworkLogic.SetCanSendPacket(value);
        }
    }
}