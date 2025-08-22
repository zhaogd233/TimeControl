
namespace Core.NetWork
{
    public class PacketDispatcher : IPacketDispatcher
    {
        public  void ReceivePacket(int messageID, System.IO.Stream packetData)
        {  
            if (messageID > 0 && messageID <= (int)MessageID.PACKETID_MAX)
            {
                NetManager.Instance.ReceivePacket((short)messageID,packetData);
            }
        }

        public bool IsCryptoPacket(short nPacketID)
        {
            return (nPacketID != (short)MessageID.PACKETID_CG_LOGIN &&
                    nPacketID != (short)MessageID.PACKETID_GC_LOGIN_RET &&
                    nPacketID != (short)MessageID.PACKETID_GC_SESSION &&
                    nPacketID != (short)MessageID.PACKETID_XX_REQUEST_HEARTBEAT &&
                    nPacketID != (short)MessageID.PACKETID_XX_RESPONSE_HEARTBEAT);
        }

        public static int GetMessageMaxID()
        {
            return (int)MessageID.PACKETID_MAX;
        }
    }
}