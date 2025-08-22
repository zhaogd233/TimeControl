using System;
using Core.EventBus;
using Core.NetWork;

namespace Core.Controller
{
    /// <summary>
    /// MVP 的P,负责处理协议，处理好的数据流向data,并广播给view,view 从p里面拉取已经筛选过的数据。
    /// </summary>
    public abstract class ControllerBase : IAutoEventHandler,IAutoPacketHandler
    {
        /// <summary>
        /// 记录注册的事件，销毁的时候自动取消订阅
        /// </summary>
        private  EventSubscriberHelper _eventHelper  = new();
        
        private   PacketHandlerHelper PacketHandlerHelper = new();
        

        #region 实现接口
        
        /// <summary>
        /// 注册需要监听的网络消息
        /// </summary>
        protected abstract void InitProtocol();
        
        /// <summary>
        /// 注册需要的监听的事件通知
        /// </summary>
        protected abstract void InitListener();


        #endregion

        #region 生命周期

        public virtual void OnInit()
        {
            InitProtocol();
            InitListener();
        }

        public virtual void OnDestroy()
        {
            UnsubscribeAllEvent();
            UnRegisterAllHandlers();
        }

        #endregion
        
        #region 共用方法

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
        
        #endregion

        /// <summary>
        /// 手动注册，自动退订
        /// </summary>
        /// <param name="handler"></param>
        /// <typeparam name="T"></typeparam>
        public void RegisterPacketHandler<T>(Action<T> handler) where T : IGCPacket
        {
            PacketHandlerHelper.RegisterPackerHander(handler);
        }

        public void UnRegisterAllHandlers()
        {
            PacketHandlerHelper.UnRegisterAllHandlers();
        }
    }
}