using Core.NetWork;
using ProtobufPacket;
using System.IO;
using Network;

namespace ProtobufPacket
{
    public partial class XX_REQUEST_HEARTBEAT : IGCPacket
	{
	}
	
	public class XX_REQUEST_HEARTBEAT_Handler : PacketHandler
	{
	    public override int GetMessageId()
        {
            return (int)MessageID.PACKETID_XX_REQUEST_HEARTBEAT;
        }
		
		public override void ReceivePacket(int messageId, Stream packetData)
		{
			XX_REQUEST_HEARTBEAT packet = XX_REQUEST_HEARTBEAT.Deserialize(packetData);
		 	NetManager.Instance.BroadcastPacket(packet);
		}
	}
}
