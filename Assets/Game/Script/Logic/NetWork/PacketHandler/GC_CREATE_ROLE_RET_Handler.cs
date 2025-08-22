using Core.NetWork;
using ProtobufPacket;
using System.IO;
using Network;

namespace ProtobufPacket
{
    public partial class GC_CREATE_ROLE_RET : IGCPacket
	{
	}
	
	public class GC_CREATE_ROLE_RET_Handler : PacketHandler
	{
	    public override int GetMessageId()
        {
            return (int)MessageID.PACKETID_GC_CREATE_ROLE_RET;
        }
		
		public override void ReceivePacket(int messageId, Stream packetData)
		{
			GC_CREATE_ROLE_RET packet = GC_CREATE_ROLE_RET.Deserialize(packetData);
		 	NetManager.Instance.BroadcastPacket(packet);
		}
	}
}
