using Core.Controller;
using Core.Data;
using Core.Obj;
using Logic.Obj;
using Logic.UserData;
using ProtobufPacket;

namespace Logic.Controller
{
    public class BattleController : ControllerBase
    {
       private CopySceneData _copySceneData;
        protected override void InitProtocol()
        {
            RegisterPacketHandler<GC_CREATE_NPC>(OnReceiveGCCreateNPC);
        }

        protected override void InitListener()
        {
        }

        public override void OnInit()
        {
            base.OnInit();
            _copySceneData = DataManager.Instance.Get<CopySceneData>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DataManager.Instance.RemoveData<CopySceneData>();
        }

        /// <summary>
        /// 创建副本NPC
        /// </summary>
        /// <param name="packet"></param>
        private void OnReceiveGCCreateNPC(GC_CREATE_NPC packet)
        {
            ObjManager.Instance.ShowObj<TCNpcLogic>(packet.OwnerId,"",GameDefine.ObjType.TCNPC,null);
        }
        
    }
}