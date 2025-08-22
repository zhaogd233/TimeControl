using System;
using System.Collections.Generic;

namespace Core.EventBus
{
    public interface IAutoEventHandler
    {
        void ListenEvent<TEvent>(EventHandler<TEvent> handler) where TEvent : IEventMessage;   // 注册监听
        void RaiseEvent<TEvent>(ref TEvent eventData) where TEvent : IEventMessage;                     // 触发事件
        void UnsubscribeAllEvent();      
    }
    
    /// <summary>
    /// 事件订阅辅助类，用于自动管理事件注册和注销
    /// </summary>
    public class EventSubscriberHelper
    {
        // 存储已注册事件的取消订阅动作
        private readonly List<Action> _unsubscribeActions = new List<Action>();

        /// <summary>
        /// 自动订阅事件，并记录取消订阅操作
        /// </summary>
        public void ListenEvent<TEvent>(EventBus.EventHandler<TEvent> callback)
            where TEvent : IEventMessage
        {
            EventManager.Instance.SubscribeEvent(callback);

            // 缓存对应的 Unsubscribe 方法
            _unsubscribeActions.Add(() =>
            {
                EventManager.Instance.UnSubscribeEvent(callback);
            });
        }

        /// <summary>
        /// 取消所有订阅（比如 FSM Exit 或 MonoBehaviour OnDestroy 时调用）
        /// </summary>
        public void UnsubscribeAll()
        {
            for (int i = 0; i < _unsubscribeActions.Count; i++)
            {
                _unsubscribeActions[i]?.Invoke();
            }
            _unsubscribeActions.Clear();
        }
    }

}