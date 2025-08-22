using Core.Controller;
using Core.Data;
using Core.FSM;
using Logic.Controller;
using UnityEngine;

namespace Logic.FSM
{
    public class Fsm_Login : FsmState
    {
        public Fsm_Login() : base((int)GameDefine.EGameState.LOGIN)
        {
        }

        public override void Enter()
        {
            ControllerManager.Instance.AddControl(new LoginController());
            //显示UI 。。。
            //TODO  UI 监听 FsmStateChanged 
            GameObject.Find("UI").transform.Find("LoginWindow").gameObject.SetActive(true);
            GameObject.Find("UI").transform.Find("Loading").gameObject.SetActive(false);
            GameObject.Find("UI").transform.Find("GMWindow").gameObject.SetActive(false);
        }

        public override void Execute(float deltaTime)
        {
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}