using Core.NetWork;
using ProtobufPacket;
using System.IO;
using Network;

namespace ProtobufPacket
{
    public partial class GC_ENTER_SCENE : IGCPacket
	{
	}
	
	public class GC_ENTER_SCENE_Handler : PacketHandler
	{
	    public override int GetMessageId()
        {
            return (int)MessageID.PACKETID_GC_ENTER_SCENE;
        }
		
		public override void ReceivePacket(int messageId, Stream packetData)
		{
			GC_ENTER_SCENE packet = GC_ENTER_SCENE.Deserialize(packetData);
		 	NetManager.Instance.BroadcastPacket(packet);
		}
	}
}
