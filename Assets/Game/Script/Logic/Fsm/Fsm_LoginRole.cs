using Core.Controller;
using Core.Data;
using Core.EventBus;
using Core.FSM;
using Logic.Controller;
using UnityEngine;

namespace Logic.FSM
{
    public enum State
    {
        NONE,
        SUCCESS,
        FAIL,
    }

    public class Fsm_LoginRole : FsmState
    {
        public string userAccount;
        State loginState = State.NONE;
        
        public Fsm_LoginRole() : base((int)GameDefine.EGameState.LOGINROLE)
        {
            ListenEvent<EventMessages.LoginEventMessage>(LoginServerResultBack);
        }

        public override void Enter()
        {
            GameObject.Find("UI").transform.Find("Loading").gameObject.SetActive(true);
            EventManager.Instance.RaiseEvent( new EventMessages.ChangeLoadingTips() {tips =  "Login Account..."});
        //    GameObject.Find("UI").transform.Find("Loading").GetComponent<LoadingWindow>().setLoadingText("Login Account...");

          ControllerManager.Instance.Get<LoginController>().SendLoginMsg(userAccount);
            loginState = State.NONE;
        }

        public override void Execute(float deltaTime)
        {
            switch ((State)loginState)
            {
                case State.SUCCESS:
                    if (ControllerManager.Instance.Get<LoginController>().IsHaveRole())
                    {
                       // FSM.ChangeState(new StateSelectRole());
                       FsmManager.Instance.ChangeState(new Fsm_SelectRole());
                    }
                    else
                    {
                        FsmManager.Instance.ChangeState(new Fsm_CreateRole(){userName = userAccount});
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

        private void LoginServerResultBack( ref EventMessages.LoginEventMessage eventdata)
        {
            if(eventdata.result == true)
              loginState = State.SUCCESS;
            else
            {
                loginState = State.FAIL;
            }
        }
    }
}