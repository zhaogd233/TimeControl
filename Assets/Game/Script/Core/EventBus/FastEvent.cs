using System;
using System.Collections.Generic;

namespace Core.EventBus
{
    public class FastEvent<T> where T : IEventMessage
    {
        private readonly List<EventHandler<T>> listeners = new List<EventHandler<T>>();

        public void Add(EventHandler<T> listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void Remove(EventHandler<T> listener)
        {
            listeners.Remove(listener);
        }

        public void Invoke(ref T arg)
        {
            // 用for循环避免GC（避免foreach生成枚举器）
            for (int i = 0; i < listeners.Count; i++)
            {
                listeners[i]?.Invoke(ref arg);
            }
        }
    }

}