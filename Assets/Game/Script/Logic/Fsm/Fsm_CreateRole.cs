using System.Collections.Generic;
using Core.Controller;
using Core.EventBus;
using Core.FSM;
using EventMessages;
using Logic.Controller;
using UnityEngine;

namespace Logic.FSM
{
    public class Fsm_CreateRole : FsmState
    {
        public string userName;
        State loginState = State.NONE;
        public Fsm_CreateRole() : base((int)GameDefine.EGameState.CREATEROLE)
        {
           ListenEvent<CreateRoleEventMessage>(OnCreateRole);
        }


        public override void Enter()
        {
           // GameObject.Find("UI").transform.Find("Loading").GetComponent<LoadingWindow>().setLoadingText("Create Role...");
           EventManager.Instance.RaiseEvent( new EventMessages.ChangeLoadingTips() {tips =  "Create Role..."});
            //Random ran = new Random();

          ControllerManager.Instance.Get<LoginController>().SendCreateRoleMsg(userName);
        }

        public override void Execute(float deltaTime)
        {
            switch ((State)loginState)
            {
                case State.SUCCESS:
                    {
                        Debug.LogError("登录进场景");
                        FsmManager.Instance.ChangeState(new Fsm_EnterSceneLoading());
                        // FSM.ChangeState(new StateSelectRole());
                    }
                    break;
                case State.FAIL:
                    Debug.Log("login fail");
                    EventManager.Instance.RaiseEvent( new EventMessages.ChangeLoadingTips() {tips =  "Repeated name..."});
              //      GameObject.Find("UI").transform.Find("Loading").GetComponent<LoadingWindow>().setLoadingText("Repeated name...");

                    FsmManager.Instance.ChangeState(new Fsm_Login());
                    break;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void OnCreateRole(ref CreateRoleEventMessage evt)
        {
            if(evt.result == true)
                loginState = State.SUCCESS;
            else
            {
                loginState = State.FAIL;
            }
        }
    }
}