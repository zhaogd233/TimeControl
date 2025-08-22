using System;

namespace Core.Obj
{
    public interface IObj
    {
        /// <summary>
        ///     获取实体编号。
        /// </summary>
        int Id { get; }

        /// <summary>
        ///     获取实体资源Name
        /// </summary>
        string AssetName { get; }

        /// <summary>
        ///     获取实体所属的实体组。
        /// </summary>
        IObjTypeGroup ObjTypeGroup { get; }

        /// <summary>
        ///     对应的逻辑实体
        /// </summary>
        ObjLogic Logic { get; }

        /// <summary>
        ///     实体初始化。
        /// </summary>
        /// <param name="ObjId">实体编号。</param>
        /// <param name="ObjAssetName">实体资源名称。</param>
        /// <param name="objTypeGroup">实体所属的实体组。</param>
        void OnInit(int ObjId, string ObjAssetName, IObjTypeGroup objTypeGroup, Type logicType);

        /// <summary>
        ///     实体显示。
        /// </summary>
        /// userData 实体相关的参数传入
        void OnShow(IShowObjLogicParams userData);

        /// <summary>
        ///     移除实体。
        /// </summary>
        void OnRemove();

        /// <summary>
        ///     回收
        /// </summary>
        void OnRecycle();

        /// <summary>
        ///     当前实体被附加子实体。
        /// </summary>
        /// <param name="childObj">附加的子实体。</param>
        void OnAttached(IObj childObj);

        /// <summary>
        ///     当前实体附加对父实体。
        /// </summary>
        /// <param name="parentObj">附加的实体。</param>
        void OnAttachTo(IObj parentObj);

        /// <summary>
        ///     当前实体解除父实体。
        /// </summary>
        /// <param name="parentObj">解除的实体。</param>
        void OnDetachFrom(IObj parentObj);

        /// <summary>
        ///     子实体解除当前实体。
        /// </summary>
        /// <param name="childObj">解除的子实体。</param>
        void OnDetached(IObj childObj);

        /// <summary>
        ///     实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        void OnUpdate(float elapseSeconds);
    }
}