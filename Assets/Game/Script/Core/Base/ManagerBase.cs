using System;

namespace Core.Base
{
    public class ManagerBase<T> : IDisposable
    {
        protected static T m_Handler = default(T);

        public static T Instance
        {
            get
            {
                if (m_Handler == null)
                {
                    m_Handler = (T)System.Activator.CreateInstance(typeof(T), true);
                }

                return m_Handler;
            }
        }

        public virtual void Tick(float deltaTime)
        {
        }
      
        public virtual void LateUpdate(float deltaTime)
        {
        }

        public virtual void OnDestroy()
        {
            Destroy();
            m_Handler = default(T);
        }

        public virtual void Destroy()
        {
        }

        public virtual void Dispose()
        {
            OnDestroy();
        }
    }
}