using Core.NetWork;
using ProtobufPacket;
using System.IO;
using Network;

namespace ProtobufPacket
{
    public partial class GC_SESSION : IGCPacket
	{
	}
	
	public class GC_SESSION_Handler : PacketHandler
	{
	    public override int GetMessageId()
        {
            return (int)MessageID.PACKETID_GC_SESSION;
        }
		
		public override void ReceivePacket(int messageId, Stream packetData)
		{
			GC_SESSION packet = GC_SESSION.Deserialize(packetData);
		 	NetManager.Instance.BroadcastPacket(packet);
		}
	}
}
