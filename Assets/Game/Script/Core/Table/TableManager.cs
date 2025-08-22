using Zeus.Framework.ConfigDB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Core.Base;

namespace Core.Table{

    public interface ITableLoader
    {
        string ReadTable(Type type);
    }
    
    public class TableManager : ModuleManager<TableManager>
    {
        private ITableLoader _tableLoaderHelper;
        private readonly  Dictionary<Type, ITable> _tables = new Dictionary<Type, ITable>();

        /// <summary>
        /// 读表器
        /// </summary>
        /// <param name="loader"></param>
        public void SetLoadAdapter(ITableLoader loader)
        {
            _tableLoaderHelper = loader;
        }
        
        public override void End()
        {
            base.End();
            ClearAllTables();
            _tableLoaderHelper = null;
        }

        /// <summary>
        /// 获取表数据（内部方法）
        /// </summary>
        private TableData<T> GetTableData<T>() where T : class,ITableBase, new()
        {
            if (!_tables.TryGetValue(typeof(T), out var obj))
            {
                var table = new TableData<T>();
                table.Init(_tableLoaderHelper.ReadTable(typeof(T)));
                _tables[typeof(T)] = table;
                return table;
            }
            return (TableData<T>)obj;
        }
        
        /// <summary>
        /// 通用的通过 ID 获取数据
        /// </summary>
        public T GetByID<T>(int nId) where T : class, ITableBase ,new()
        {
            var table = GetTableData<T>();
            if (nId < 0 || table.IsNullData(nId))
                return null;

            var data = table.GetDataByKey(nId);
            if (data == null)
                table.AddNullDataID(nId);

            return data;
        }
        
        // <summary>
        /// 通用的获取整个表
        /// </summary>
        public IDataTable<T> GetTable<T>() where T :  class, ITableBase ,new()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            return GetTableData<T>();
#else
        return null;
#endif
        }
        
        /// <summary>
        /// 清除指定表
        /// </summary>
        public void ClearTable<T>() where T :  class, ITableBase ,new()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (_tables.TryGetValue(typeof(T), out var obj))
                ((TableData<T>)obj).Clear();
#endif
        }

        /// <summary>
        /// 清除所有表
        /// </summary>
        public void ClearAllTables()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            foreach (ITable obj in _tables.Values)
            {
                obj.Clear();
            }
            _tables.Clear();
#endif
        }
    }
}
