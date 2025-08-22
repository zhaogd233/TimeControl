using System.Collections.Generic;
using System.Runtime.Serialization;
using Core.Base;
using Core.EventBus;
using Core.FSM;
using Core.Obj;
using Core.Table;
using Cysharp.Threading.Tasks;
using EventBus;
using EventMessages;
using Game.Script.Logic.Resource;
using Games.Table;
using Logic.Obj;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Zeus.Framework.ConfigDB;

namespace Logic.FSM
{
    public class Fsm_EnterSceneLoading : FsmState
    {
        private bool bEnterScene = false;
        private int nTargetSceneId = -1;
        private Camera uiCamera;

        public Fsm_EnterSceneLoading() : base((int)GameDefine.EGameState.ENTERWORLDLOADING)
        {
            CreateOBJManager();
            ListenEvent<EnterSceneEventMessage>(onEnterSceneSuccess);
        }


        public override void Enter()
        {
            EventManager.Instance.RaiseEvent( new EventMessages.ChangeLoadingTips() {tips =  "Enter Scene..."});
            GameObject.DontDestroyOnLoad(GameObject.Find("UI"));
            uiCamera = GameObject.Find("UI").transform.Find("UICamera").GetComponent<Camera>();
        }

        public override async void Execute(float deltaTime)
        {
            if (bEnterScene)
            {
                bEnterScene = false;
              
                string sceneResourceName = GetSceneResourceName(nTargetSceneId);
                LogModule.Log($"准备加载场景: ID={nTargetSceneId}, Resource={sceneResourceName}");
                await LoadSceneAsync(sceneResourceName);
                FsmState fsmInst = GetWroldSceneFsmInst(nTargetSceneId);
                FsmManager.Instance.ChangeState(new Fsm_GameWorld(){EnterState = fsmInst});
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            GameObject.Find("UI").transform.Find("LoginWindow").gameObject.SetActive(false);
            GameObject.Find("UI").transform.Find("Loading").gameObject.SetActive(false);
            GameObject.Find("UI").transform.Find("GMWindow").gameObject.SetActive(false);
        }

        private void onEnterSceneSuccess(ref EnterSceneEventMessage evt)
        {
            nTargetSceneId = evt.sceneId;
            bEnterScene = true;
        }
        
        private void CreateOBJManager()
        {
            GameModuleManager.Instance.RegisterModuleAndRun(ObjManager.Instance);
            
            if(GameObject.Find("ObjGroup")) return;
            
            GameObject objGroup = new GameObject("ObjGroup");
            GameObject.DontDestroyOnLoad(objGroup);
            ObjManager.Instance.SetHelper(new AssetDataBaseHelper(),new DefaultObjHelper());

            IDataTable<Tab_ObjTypeConfig> allConfig = TableManager.Instance.GetTable<Tab_ObjTypeConfig>();

            foreach (KeyValuePair<int,Tab_ObjTypeConfig> config in allConfig)
            {
                ObjManager.Instance.AddObjGroup(config.Value.Name,objGroup.transform);
            }
        }
        
        public  async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            // 开始加载场景
            var op = SceneManager.LoadSceneAsync(sceneName, mode);
            op.allowSceneActivation = true;

            // 等待加载完成
            await op.ToUniTask(Progress.Create<float>(p => Debug.Log($"Loading {sceneName}: {p:P0}")));

            // 等待主相机生成（有时场景里的 MainCamera 要等 1 帧才生效）
            Camera baseCamera = null;
            await UniTask.WaitUntil(() =>
            {
                baseCamera = Camera.main;
                return baseCamera != null;
            });

            // 把 UI 相机加到 base camera 的 stack
            var urpCam = baseCamera.GetUniversalAdditionalCameraData();
            if (!urpCam.cameraStack.Contains(uiCamera))
            {
                urpCam.cameraStack.Add(uiCamera);
            }

            Debug.Log($"Scene {sceneName} loaded, UICamera added to stack.");
        }
        
        public  string GetSceneResourceName(int sceneId)
        {
            var sceneConfig = GetSceneConfig(sceneId);
            if (sceneConfig != null)
            {
                int sceneResourceId = sceneConfig.SceneResource;
                var resourceConfig = TableManager.Instance.GetByID<Tab_SceneResource>(sceneResourceId);
                if (resourceConfig != null)
                {
                    string resourceName = resourceConfig.ResName;
                    LogModule.Log($"Scene {sceneId} -> SceneResource {sceneResourceId} -> Resource: {resourceName}");
                    return resourceName;
                }
            }
            return "Demo_Room";
        }

        public  FsmState GetWroldSceneFsmInst(int sceneId)
        {
            var sceneConfig = GetSceneConfig(sceneId);
            if (sceneConfig != null)
            {
                switch (sceneConfig.Type)
                {
                    case 0:
                        return new Fsm_Home();
                    case 2:
                        return new Fsm_Battle();
                    default:
                        return new Fsm_Home();
                }
            }
            return new Fsm_Home();
        }

        public  Tab_SceneClass GetSceneConfig(int sceneId)
        {
            var sceneConfig = TableManager.Instance.GetByID<Tab_SceneClass>(sceneId);
            if (sceneConfig != null)
            {
                return sceneConfig;
            }
            return null;
        }
    }
}