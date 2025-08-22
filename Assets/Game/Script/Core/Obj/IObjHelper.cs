using UnityEngine;

namespace Core.Obj
{
    public interface IObjHelper
    {
        /// <summary>
        /// 实例化实体。
        /// </summary>
        /// <param name="ObjAsset">要实例化的实体资源。</param>
        /// <returns>实例化后的实体。</returns>
        Object InstantiateObj(Object ObjAsset);

        /// <summary>
        /// 创建实体。
        /// </summary>
        /// <param name="ObjInstance">实体实例。</param>
        /// <param name="ObjGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>实体。</returns>
        IObj CreateObj(Object ObjInstance, IObjTypeGroup ObjGroup, object userData);

        /// <summary>
        /// 释放实体。
        /// </summary>
        /// <param name="ObjAsset">要释放的实体资源。</param>
        /// <param name="ObjInstance">要释放的实体实例。</param>
        void ReleaseObj(Object ObjAsset, Object ObjInstance);
    }
}