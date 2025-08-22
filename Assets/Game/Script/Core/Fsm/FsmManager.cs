using Core.Base;
using Core.EventBus;
using UnityEngine;

namespace Core.FSM
{
    public struct FsmStateChanged : IEventMessage
    {
        public int newState;
    }
    public class FsmManager : ModuleManager<FsmManager>
    {
        private FsmState _currentState    = null;
        private FsmState _previousState   = null;
        public override void Start()
        {
            base.Start();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
            
            if(_currentState != null)
                _currentState.Execute(deltaTime);
        }

        public override void End()
        {
            base.End();
            if(_currentState != null)
                _currentState.Exit();
            _currentState = null;
        }
        
        public void ChangeState(FsmState newState)
        {
            if (null == newState)
            {
                Debug.LogError("new state is null");
                return;
            }

            _previousState = _currentState;
            if (null != _currentState)
            {
                _currentState.Exit();
            }
            _currentState = newState;
            _currentState.Enter();
            
            EventManager.Instance.RaiseEvent(new FsmStateChanged(){newState = _currentState.CurrentState});
        }
        
        public void ResetToPreviousState()
        {
            ChangeState(_previousState);   
        }
        
        public FsmState CurState()
        {
            return _currentState;
        }
    }
}