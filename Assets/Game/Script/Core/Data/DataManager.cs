using System;
using System.Collections.Generic;
using Core.Base;
using EventBus;
using UnityEngine;

namespace Core.Data
{
    public class DataManager : ModuleManager<DataManager>
    {
        private readonly Dictionary<Type, DataBase> _dataMap = new();
        public override void Start()
        {
            base.Start();
        }

        public override void End()
        {
            base.End();

            foreach (KeyValuePair<Type,DataBase> keyValuePair in _dataMap)
            {
                try
                {
                    keyValuePair.Value.UnloadData();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            _dataMap.Clear();
        }
        
        public T Get<T>() where T :  DataBase
        {
            if(_dataMap.TryGetValue(typeof(T), out DataBase dataBase))
                return _dataMap[typeof(T)] as T;
            else
            {
               var value = (T)System.Activator.CreateInstance(typeof(T), true);
               AddData(value);
               return value;
            }
        }

        public void AddData<T>(T data) where T : DataBase
        {
            data.LoadData();
            _dataMap[typeof(T)] = data;
        }

        public void RemoveData<T>() where T : DataBase
        {
            if (_dataMap.TryGetValue(typeof(T), out var data))
            {
                data.UnloadData();
                _dataMap.Remove(typeof(T));
            }
        }
    }
}