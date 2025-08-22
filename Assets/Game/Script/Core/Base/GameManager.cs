using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Base
{
    public class GameModuleManager : ManagerBase<GameModuleManager>
    {
        private List<IModuel> _moduleManager = new List<IModuel>();

      
    public void RegisterModuleAndRun(IModuel module)
    {
        if (!_moduleManager.Contains(module))
            _moduleManager.Add(module);

        module.Start();
    }

    public void UnRegisterModule(IModuel moduel)
    {
        if (moduel == null || !_moduleManager.Contains(moduel))
            return;
        
        moduel.End();
        _moduleManager.Remove(moduel);
    }

    public override void Destroy()
    {
        try
        {
            for (int i = _moduleManager.Count - 1; i >= 0; i--)
            {
                if (_moduleManager[i] != null)
                {
                    _moduleManager[i].End();
                    _moduleManager.Remove(_moduleManager[i]);
                }
            }
            _moduleManager.Clear();
            
            base.Destroy();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    public override void Tick(float deltaTime)
    {
        for (int i = 0 ; i < _moduleManager.Count; ++i)
        {
            if(!ReferenceEquals(_moduleManager[i],null))
                try
                {
                    _moduleManager[i].Tick(deltaTime);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
        }
    }

    public override void LateUpdate(float deltaTime)
    {
        for (int i = 0 ; i < _moduleManager.Count; ++i)
        {
            if(!ReferenceEquals(_moduleManager[i],null))
                try
                {
                    _moduleManager[i].LateUpdate(deltaTime);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
        }
    }

    public void DebugGUI()
    {
        for (int i = 0; i < _moduleManager.Count; ++i)
        {
            if(!ReferenceEquals(_moduleManager[i],null))
                _moduleManager[i].DebugGUI();
        }
    }
    }
}