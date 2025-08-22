using System.Collections.Generic;
using Core.Base;
using Core.Controller;
using Core.FSM;
using Core.TVA;
using Logic.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zeus.Framework.ConfigDB;

namespace Logic.FSM
{
    public class Fsm_Battle : FsmState
    {
        public Fsm_Battle() : base((int)GameDefine.EWorldState.BATTLE)
        {
        }

        public override  void Enter()
        {
            ControllerManager.Instance.AddControl(new BattleController());
            
            GameObject.Find("UI").transform.Find("GMWindow").gameObject.SetActive(true);

            //启动时间回溯模块
            GameModuleManager.Instance.RegisterModuleAndRun(TCManager.Instance);
            
            var transform = GameObject.Find("Battle").transform;
            for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(true);

            
            /*
            ObjManager.Instance.ShowObj<TCActorLogic>(1,"NPC",Logic.Obj.Define.ObjType.TCActor,null); 
            
            IObj obj = await ObjManager.Instance.ShowObjAwait<TCActorLogic>(2, "NPC", Define.ObjType.TCActor, null);
            LogModule.Log(ObjManager.Instance.GetObj(2).Logic.Available);*/
        }

        public override void Execute(float deltaTime)
        {
        }

        public override void Exit()
        {
        }
    }
}