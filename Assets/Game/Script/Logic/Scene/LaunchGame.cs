using Core.Base;
using Core.FSM;
using Logic.FSM;
using UnityEngine;

namespace Logic.Scene
{
    public class LaunchGame : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            //进入游戏
            FsmManager.Instance.ChangeState(new Fsm_Initialized());
        }

        private void Update()
        {
            GameModuleManager.Instance.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            GameModuleManager.Instance.LateUpdate(Time.deltaTime);
        }

        private void OnDestroy()
        {
            GameModuleManager.Instance.Destroy();
        }
    }
}