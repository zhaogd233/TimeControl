using Core.Controller;
using Core.Data;
using Core.EventBus;
using EventMessages;
using Logic.UserData;
using ProtobufPacket;

namespace Logic.Controller
{
    public class GameControllder : ControllerBase
    {
        private GlobalData _globalData;
        protected override void InitProtocol()
        {
            RegisterPacketHandler<XX_REQUEST_HEARTBEAT>(OnReceiveRequestHeartBeat);
            RegisterPacketHandler<GC_ENTER_SCENE>(OnRecEnterSceneMsg);
        }

        protected override void InitListener()
        {
        }

        public override void OnInit()
        {
            base.OnInit();
            _globalData = DataManager.Instance.Get<GlobalData>();
        }

        private void OnReceiveRequestHeartBeat(XX_REQUEST_HEARTBEAT packet)
        {
            _globalData.ServerAnsiTime = packet.ansi_time;
            XX_RESPONSE_HEARTBEAT_PAK response = new XX_RESPONSE_HEARTBEAT_PAK();
            response.SendPacket();
        }
        
        private void OnRecEnterSceneMsg(GC_ENTER_SCENE packet)
        {
            // 传递场景ID到事件中，使用sceneclass字段
            var eventMessage = new EnterSceneEventMessage { sceneId = packet.sceneclass };
            EventManager.Instance.RaiseEvent(eventMessage);
        }

        public void OnSendGMMsg(string msg)
        {
            CG_GMCMDSTR_PAK packet = new CG_GMCMDSTR_PAK();
            packet.data.cmdstr = msg;
            packet.SendPacket();
        }
    }
}