using Core.EventBus;
using Core.FSM;
using EventMessages;
using GameDefine;

namespace Logic.FSM
{
    public class Fsm_GameWorld : FsmState
    {
        private FsmManager _subWorldManager;
        public FsmState EnterState; //进入大世界的默认state

        public Fsm_GameWorld() : base((int)EGameState.GAMEWORLD)
        {
            EventManager.Instance.SubscribeEvent<ChangeWorldStateMsg>(ChangeWorldState);
        }

        public override void Enter()
        {
            _subWorldManager = new FsmManager();
            _subWorldManager.Start();
            _subWorldManager.ChangeState(EnterState);
        }

        public override void Execute(float deltaTime)
        {
            _subWorldManager.Tick(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            _subWorldManager.End();
            _subWorldManager = null;
        }

        private void ChangeWorldState(ref ChangeWorldStateMsg msg)
        {
            _subWorldManager.ChangeState(msg.worldState);
        }
    }
}