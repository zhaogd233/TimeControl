namespace Core.Base
{
    
    public interface IModuel
    {
        void Start();
        void End();
        void Pause();
        void Resume();
        void Tick(float deltaTime);
        void LateUpdate(float deltaTime);

        //调试使用
        void DebugGUI();
    }
    
    public class ModuleManager<T>: ManagerBase<T>, IModuel
    {
        
         public virtual void Start() { }
         public virtual void End() { OnDestroy(); }
         public virtual void Pause() { }
         public virtual void Resume() { }
         
         /// <summary>
         /// update 更新时机
         /// </summary>
         /// <param name="deltaTime"></param>
         public new virtual void Tick(float deltaTime)
        {
            base.Tick(deltaTime);
        }

        public new virtual void LateUpdate(float deltaTime)
        {
            base.LateUpdate(deltaTime);
        }

         public virtual void DebugGUI() { }

         public sealed override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}