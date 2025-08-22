namespace Core.NetWork
{
    public abstract class PacketHandler
    {
        public abstract int GetMessageId();
        public abstract void ReceivePacket(int messageId,System.IO.Stream packetData);

    }
}