using System.IO;

namespace Core.NetWork
{
    public interface IPacketDispatcher
    {
        void ReceivePacket(int messageID, Stream packetData);
        bool IsCryptoPacket(short nPacketID);
    }
}