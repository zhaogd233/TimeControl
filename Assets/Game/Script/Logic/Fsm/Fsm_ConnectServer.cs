using Core.EventBus;
using Core.FSM;
using Core.NetWork;
using EventMessages;
using GameDefine;
using UnityEngine;

namespace Logic.FSM
{
    public class Fsm_ConnectServer : FsmState
    {
        public string ip;
        private bool m_bConnectFinish;
        private bool m_bConnectSuccess;
        public int port;
        public string userAccount;

        public Fsm_ConnectServer() : base((int)EGameState.CONNECTSERVER)
        {
        }

        public override void Enter()
        {
            NetManager.Instance.ConnectToServer(ip, port, true, OnConnectServer);
            EventManager.Instance.RaiseEvent(new ChangeLoadingTips { tips = "connect Server..." });
        }

        public override void Execute(float deltaTime)
        {
            if (m_bConnectFinish)
            {
                if (m_bConnectSuccess)
                {
                    Debug.Log(": connect server success");
                    FsmManager.Instance.ChangeState(new Fsm_LoginRole { userAccount = userAccount });
                }
                else
                {
                    Debug.Log(": connect server fail");
                    FsmManager.Instance.ResetToPreviousState();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void OnConnectServer(bool bSuccess)
        {
            m_bConnectFinish = true;
            m_bConnectSuccess = bSuccess;
        }
    }
}