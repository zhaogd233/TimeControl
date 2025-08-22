namespace Core.NetWork
{
    public abstract class ProtoPacket
    {
        public abstract short GetMessageID();
        public abstract byte[] GetPacketArray();
        public void SendPacket()
        {
            NetworkLogic.SendPacket(this);
        }
    }
}