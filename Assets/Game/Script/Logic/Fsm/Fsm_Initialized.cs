using Core.Base;
using Core.Controller;
using Core.Data;
using Core.EventBus;
using Core.FSM;
using Core.NetWork;
using Core.Table;
using Games.Table;
using Logic.Controller;
using UnityEngine;

namespace Logic.FSM
{
    public class Fsm_Initialized : FsmState
    {
        public Fsm_Initialized() : base((int) GameDefine.EGameState.INITIALIZED)
        {
           
        }

        public override void Enter()
        {
            //全局manager
            Application.targetFrameRate = 30;
            TableManager.Instance.SetLoadAdapter(new TxTTableLoader());
            GameModuleManager.Instance.RegisterModuleAndRun(TableManager.Instance);
            GameModuleManager.Instance.RegisterModuleAndRun(EventManager.Instance);

            NetManager.Instance.SetPlatformAdapter(new PacketDispatcher(), new NetPlatformAdapter());
            GameModuleManager.Instance.RegisterModuleAndRun(NetManager.Instance);
            GameModuleManager.Instance.RegisterModuleAndRun(FsmManager.Instance);
            GameModuleManager.Instance.RegisterModuleAndRun(DataManager.Instance);
            GameModuleManager.Instance.RegisterModuleAndRun(ControllerManager.Instance);
            
            ControllerManager.Instance.AddControl(new GameControllder());
        }

        public override void Execute(float deltaTime)
        {
            FsmManager.Instance.ChangeState(new Fsm_Login());
        }
    }
}