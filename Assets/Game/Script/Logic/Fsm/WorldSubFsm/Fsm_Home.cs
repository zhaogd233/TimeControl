using System.Collections.Generic;
using Core.Base;
using Core.Controller;
using Core.Data;
using Core.FSM;
using Logic.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zeus.Framework.ConfigDB;

namespace Logic.FSM
{
    public class Fsm_Home : FsmState
    {
        private Camera uiCamera;
        public Fsm_Home() : base((int)GameDefine.EWorldState.HOME)
        {
        }

        public override  void Enter()
        {
            ControllerManager.Instance.AddControl(new HomeController());
            GameObject.Find("UI").transform.Find("JoyStick").gameObject.SetActive(true);
            GameObject.Find("UI").transform.Find("GMWindow").gameObject.SetActive(true);
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