using Core.Obj;
using Core.Utility;
using UnityEngine;

namespace Logic.Obj
{
    public class DefaultObjHelper : IObjHelper
    {
        /// <summary>
        /// 实例化实体。
        /// </summary>
        /// <param name="ObjAsset">要实例化的实体资源。</param>
        /// <returns>实例化后的实体。</returns>
        public  Object InstantiateObj(Object ObjAsset)
        {
            return GameObject.Instantiate((Object)ObjAsset);
        }

        /// <summary>
        /// 创建实体。
        /// </summary>
        /// <param name="ObjInstance">实体实例。</param>
        /// <param name="ObjGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>实体。</returns>
        public  IObj CreateObj(Object ObjInstance, IObjTypeGroup ObjGroup, object userData)
        {
            GameObject gameObject = ObjInstance as GameObject;

            Transform transform = gameObject.transform;
            transform.SetParent(ObjGroup.Handle);

            return gameObject.GetOrAddComponent<Core.Obj.Obj>();
        }

        /// <summary>
        /// 释放实体。
        /// </summary>
        /// <param name="ObjAsset">要释放的实体资源。</param>
        /// <param name="ObjInstance">要释放的实体实例。</param>
        public  void ReleaseObj(Object ObjAsset, Object ObjInstance)
        {
            GameObject.Destroy((Object)ObjInstance);
        }
    }
}