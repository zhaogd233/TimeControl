using System;
using Core.EventBus;
using Core.NetWork;

namespace Core.Data
{
    public abstract class DataBase
    {
        /// <summary>
        /// 从本地读取保存的数据
        /// </summary>
        protected virtual void LoadLocalData(){}

        protected virtual void SaveLocalData(){}

        protected abstract void InitData();
        protected abstract void ClearData();

        #region 生命周期

        public virtual void LoadData()
        {
            InitData();
            LoadLocalData();
        }

        public virtual void UnloadData()
        {
            SaveLocalData();
            ClearData();
        }
        #endregion
    }
}