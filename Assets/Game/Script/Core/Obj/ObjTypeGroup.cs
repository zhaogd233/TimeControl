using System.Collections.Generic;
using UnityEngine;

namespace Core.Obj
{
    public class ObjTypeGroup : IObjTypeGroup
    {
        private readonly LinkedList<IObj> m_Objs;
        private LinkedListNode<IObj> m_CachedNode;

        /// <summary>
        ///     初始化实体组的新实例。
        /// </summary>
        /// <param name="name">实体组名称。</param>
        public ObjTypeGroup(string name, Transform transform)
        {
            Name = name;
            Handle = transform;
            m_Objs = new LinkedList<IObj>();
            m_CachedNode = null;
        }

        /// <summary>
        ///     获取实体组名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     获取实体组中实体数量。
        /// </summary>
        public int ObjCount => m_Objs.Count;

        public Transform Handle { get; }

        /// <summary>
        ///     实体组中是否存在实体。
        /// </summary>
        /// <param name="ObjId">实体序列编号。</param>
        /// <returns>实体组中是否存在实体。</returns>
        public bool HasObj(int ObjId)
        {
            foreach (var Obj in m_Objs)
                if (Obj.Id == ObjId)
                    return true;

            return false;
        }

        /// <summary>
        ///     从实体组中获取实体。
        /// </summary>
        /// <param name="ObjId">实体序列编号。</param>
        /// <returns>要获取的实体。</returns>
        public IObj GetObj(int ObjId)
        {
            foreach (var Obj in m_Objs)
                if (Obj.Id == ObjId)
                    return Obj;

            return null;
        }

        /// <summary>
        ///     从实体组中获取实体。
        /// </summary>
        /// <param name="ObjAssetName">实体资源名称。</param>
        /// <returns>要获取的实体。</returns>
        public IObj[] GetObjs(string ObjAssetName)
        {
            var results = new List<IObj>();
            foreach (var Obj in m_Objs)
                if (Obj.AssetName == ObjAssetName)
                    results.Add(Obj);

            return results.ToArray();
        }

        /// <summary>
        ///     从实体组中获取实体。
        /// </summary>
        /// <param name="ObjAssetName">实体资源名称。</param>
        /// <param name="results">要获取的实体。</param>
        public void GetObjs(string ObjAssetName, List<IObj> results)
        {
            results.Clear();
            foreach (var Obj in m_Objs)
                if (Obj.AssetName == ObjAssetName)
                    results.Add(Obj);
        }

        /// <summary>
        ///     从实体组中获取所有实体。
        /// </summary>
        /// <returns>实体组中的所有实体。</returns>
        public IObj[] GetAllObjs()
        {
            var results = new List<IObj>();
            foreach (var Obj in m_Objs) results.Add(Obj);

            return results.ToArray();
        }

        /// <summary>
        ///     从实体组中获取所有实体。
        /// </summary>
        /// <param name="results">实体组中的所有实体。</param>
        public void GetAllObjs(List<IObj> results)
        {
            results.Clear();
            foreach (var Obj in m_Objs) results.Add(Obj);
        }

        /// <summary>
        ///     实体组轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds)
        {
            var current = m_Objs.First;
            while (current != null)
            {
                m_CachedNode = current.Next;
                current.Value.OnUpdate(elapseSeconds);
                current = m_CachedNode;
                m_CachedNode = null;
            }
        }

        /// <summary>
        ///     往实体组增加实体。
        /// </summary>
        /// <param name="Obj">要增加的实体。</param>
        public void AddObj(IObj Obj)
        {
            m_Objs.AddLast(Obj);
        }

        /// <summary>
        ///     从实体组移除实体。
        /// </summary>
        /// <param name="Obj">要移除的实体。</param>
        public void RemoveObj(IObj Obj)
        {
            if (m_CachedNode != null && m_CachedNode.Value == Obj) m_CachedNode = m_CachedNode.Next;

            m_Objs.Remove(Obj);
        }

        #region 复用回收同类型的对象

        /// <summary>
        ///     TODO 从pool 复用obj
        /// </summary>
        /// <param name="AssetName"></param>
        /// <returns></returns>
        public Obj SpawnObj(string AssetName)
        {
            return null;
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="Obj"></param>
        public void UnSpawnObj(IObj Obj)
        {
        }

        #endregion
    }
}