using System.Collections.Generic;
using UnityEngine;

namespace Core.Obj
{
    public interface IObjTypeGroup
    {
        /// <summary>
        ///     获取实体组名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     获取实体组中实体数量。
        /// </summary>
        int ObjCount { get; }

        /// <summary>
        ///     group 所在的Trans节点
        /// </summary>
        Transform Handle { get; }

        /// <summary>
        ///     实体组中是否存在实体。
        /// </summary>
        /// <param name="ObjId">实体序列编号。</param>
        /// <returns>实体组中是否存在实体。</returns>
        bool HasObj(int ObjId);

        /// <summary>
        ///     从实体组中获取实体。
        /// </summary>
        /// <param name="ObjId">实体序列编号。</param>
        /// <returns>要获取的实体。</returns>
        IObj GetObj(int ObjId);

        /// <summary>
        ///     从实体组中获取实体。
        /// </summary>
        /// <param name="ObjAssetName">实体资源。</param>
        /// <returns>要获取的实体。</returns>
        IObj[] GetObjs(string ObjAssetName);

        /// <summary>
        ///     从实体组中获取实体。
        /// </summary>
        /// <param name="ObjAssetName">实体资源。</param>
        /// <param name="results">要获取的实体。</param>
        void GetObjs(string ObjAssetName, List<IObj> results);

        /// <summary>
        ///     从实体组中获取所有实体。
        /// </summary>
        /// <returns>实体组中的所有实体。</returns>
        IObj[] GetAllObjs();

        /// <summary>
        ///     从实体组中获取所有实体。
        /// </summary>
        /// <param name="results">实体组中的所有实体。</param>
        void GetAllObjs(List<IObj> results);
    }
}