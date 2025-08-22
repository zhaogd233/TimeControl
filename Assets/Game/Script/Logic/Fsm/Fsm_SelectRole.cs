using Core.Controller;
using Core.Data;
using Core.EventBus;
using Core.FSM;
using EventBus;
using Logic.Controller;
using TMPro;
using UnityEngine;

namespace Logic.FSM
{
    public class Fsm_SelectRole : FsmState
    {
        State LoginState = (int)State.NONE;
        public Fsm_SelectRole() : base((int)GameDefine.EGameState.SELECTROLE)
        {
            ListenEvent<EventMessages.SelecteRoleEventMessage>(OnSelectRoleFinish);
        }


        public override void Enter()
        {
            EventManager.Instance.RaiseEvent( new EventMessages.ChangeLoadingTips() {tips =  "Select Role..."});
            LoginState = (int)State.NONE;
            ControllerManager.Instance.Get<LoginController>().SendRandomRoleMsg();
        }

        public override void Execute(float deltaTime)
        {
            switch ((State)LoginState)
            {
                case State.SUCCESS:
                {
                    FsmManager.Instance.ChangeState(new Fsm_EnterSceneLoading());
                }
                    break;
                case State.FAIL:
                    Debug.Log("login fail");
                    FsmManager.Instance.ChangeState(new Fsm_Login());
                    break;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void OnSelectRoleFinish(ref EventMessages.SelecteRoleEventMessage eventdata)
        {
            if(eventdata.result == true)
                LoginState = State.SUCCESS;
            else
            {
                LoginState = State.FAIL;
            }
        }
    }
}