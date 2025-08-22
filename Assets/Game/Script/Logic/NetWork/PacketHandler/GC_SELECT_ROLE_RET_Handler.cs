using System.IO;
using Core.NetWork;

namespace ProtobufPacket
{
    public partial class GC_SELECT_ROLE_RET : IGCPacket
    {
    }

    public class GC_SELECT_ROLE_RET_Handler : PacketHandler
    {
        public override int GetMessageId()
        {
            return (int)MessageID.PACKETID_GC_SELECT_ROLE_RET;
        }

        public override void ReceivePacket(int messageId, Stream packetData)
        {
            var packet = GC_SELECT_ROLE_RET.Deserialize(packetData);
            NetManager.Instance.BroadcastPacket(packet);
        }
    }
}