using System;
using System.Collections.Generic;
using Core.Base;
using UnityEngine;

namespace Core.Controller
{
    public class ControllerManager : ModuleManager<ControllerManager>
    {
        private readonly Dictionary<Type, ControllerBase> _ctrlMap = new();
        public override void Start()
        {
            base.Start();
        }

        public override void End()
        {
            base.End();

            foreach (KeyValuePair<Type,ControllerBase> keyValuePair in _ctrlMap)
            {
                try
                {
                    keyValuePair.Value.OnDestroy();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            _ctrlMap.Clear();
        }
        
        public T Get<T>() where T :  ControllerBase
        {
            return _ctrlMap[typeof(T)] as T;
        }

        public void AddControl<T>(T ctrl) where T : ControllerBase
        {
            if(!_ctrlMap.ContainsKey(typeof(T)))
            {
                ctrl.OnInit();
                _ctrlMap[typeof(T)] = ctrl;
            }
        }

        public void RemoveControl<T>() where T : ControllerBase
        {
            if (_ctrlMap.TryGetValue(typeof(T), out var data))
            {
                data.OnDestroy();
                _ctrlMap.Remove(typeof(T));
            }
        }
    }
}