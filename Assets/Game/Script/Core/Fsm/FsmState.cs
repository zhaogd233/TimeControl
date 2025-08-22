using System;
using System.Collections.Generic;
using Core.EventBus;
using EventBus;

namespace Core.FSM
{
    public abstract class FsmState : IAutoEventHandler
    {
        public int CurrentState { get; private set; }
        /// <summary>
        /// 记录注册的事件，销毁的时候自动取消订阅
        /// </summary>
        private readonly EventSubscriberHelper _eventHelper  = new();
        public FsmState(int type)
        {
            CurrentState = type;
        } 

        public abstract void Enter();
        public abstract void Execute(float deltaTime);

        public virtual void Exit()
        {
            UnsubscribeAllEvent();
        }
        
        // <summary>
        /// 注册事件并记录对应的注销操作（销毁的时候自动取消注册的事件）
        /// </summary>
        public void ListenEvent<TEvent>(EventBus.EventHandler<TEvent> handler) where TEvent : IEventMessage
        {
            _eventHelper.ListenEvent(handler);
        }

        public void RaiseEvent<TEvent>(ref TEvent eventData) where TEvent : IEventMessage
        {
            int eventId = typeof(TEvent).GetHashCode();
            EventManager.Instance.RaiseEventInternal(eventId,ref eventData);
        }

        public void UnsubscribeAllEvent()
        {
            _eventHelper.UnsubscribeAll();
        }


    }
}