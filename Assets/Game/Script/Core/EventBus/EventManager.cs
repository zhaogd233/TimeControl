using System;
using System.Collections.Generic;
using Core.Base;
using UnityEngine;

namespace Core.EventBus
{
    public delegate void EventHandler<TEvent>(ref TEvent eventData) where TEvent : IEventMessage;

    public class EventManager : ModuleManager<EventManager>
    {
        private static readonly Dictionary<int, object> eventTable = new();
        private static readonly object locker = new();

        public override void Start()
        {
            base.Start();
        }

        public void SubscribeEvent<T>(EventHandler<T> listener) where T : IEventMessage
        {
            var eventId = typeof(T).GetHashCode();
            SubscribeEvent(eventId, listener);
        }

        public void SubscribeEvent<T>(int eventId, EventHandler<T> listener) where T : IEventMessage
        {
            if (listener == null)
            {
                Debug.LogError("AddListener: listener is null!");
                return;
            }

            lock (locker)
            {
                if (!eventTable.TryGetValue(eventId, out var obj))
                {
                    var newEvent = new FastEvent<T>();
                    eventTable[eventId] = newEvent;
                    newEvent.Add(listener);
                }
                else
                {
                    ((FastEvent<T>)obj).Add(listener);
                }
            }
        }

        public void UnSubscribeEvent<T>(EventHandler<T> listener) where T : IEventMessage
        {
            var eventId = typeof(T).GetHashCode();
            UnSubscribeEvent(eventId, listener);
        }

        public void UnSubscribeEvent<T>(int eventId, EventHandler<T> listener) where T : IEventMessage
        {
            if (listener == null)
            {
                Debug.LogError("RemoveListener: listener is null!");
                return;
            }

            lock (locker)
            {
                if (eventTable.TryGetValue(eventId, out var obj))
                    ((FastEvent<T>)obj).Remove(listener);
                else
                    Debug.LogWarning($"RemoveListener: no listeners found for event type {eventId}");
            }
        }

        public void RaiseEvent<T>(T eventData) where T : IEventMessage
        {
            var eventId = typeof(T).GetHashCode();
            RaiseEventInternal(eventId, ref eventData);
        }

        public void RaiseEvent<T>(int eventId, T eventData) where T : IEventMessage
        {
            RaiseEventInternal(eventId, ref eventData);
        }

        public void RaiseEventInternal<T>(int eventId, ref T eventData) where T : IEventMessage
        {
            if (eventTable.TryGetValue(eventId, out var obj))
                try
                {
                    ((FastEvent<T>)obj).Invoke(ref eventData);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
        }

        public override void End()
        {
            base.End();
        }
    }
}