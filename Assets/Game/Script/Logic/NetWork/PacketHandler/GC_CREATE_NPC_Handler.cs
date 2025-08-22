using Core.NetWork;
using ProtobufPacket;
using System.IO;

namespace ProtobufPacket
{
    public partial class GC_CREATE_NPC : IGCPacket
	{
	}
	
	public class GC_CREATE_NPC_Handler : PacketHandler
	{
	    public override int GetMessageId()
        {
            return (int)MessageID.PACKETID_GC_CREATE_NPC;
        }
		
		public override void ReceivePacket(int messageId, Stream packetData)
		{
			GC_CREATE_NPC packet = GC_CREATE_NPC.Deserialize(packetData);
		 	NetManager.Instance.BroadcastPacket(packet);
		}
	}
}
