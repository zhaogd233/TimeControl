using System;
using System.Collections.Generic;
using Core.Base;
using Core.EventBus;
using Core.Resource;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Obj
{
    public struct CreateObjSuccessEventArgs : IEventMessage
    {
        public int ObjId;
        public IObj Obj;
    }

    public struct CreateObjFailedEventArgs : IEventMessage
    {
        public int ObjId;
    }

    /// <summary>
    ///     大世界内的所有3d 对象都是实体包括特效
    /// </summary>
    public partial class ObjManager : ModuleManager<ObjManager>
    {
        private LoadAssetCallbacks m_LoadAssetCallbacks;
        private Dictionary<string, ObjTypeGroup> m_ObjGroups;
        private IObjHelper m_ObjHelper;
        private Dictionary<int, ObjInfo> m_ObjInfos; //实体&附加信息 包含附加的子实体&parent
        private HashSet<int> m_ObjsBeingLoaded; // 正在loading 中的实体
        private HashSet<int> m_ObjsToReleaseOnLoad; // 即将释放的实体
        private Queue<ObjInfo> m_RecycleQueue;
        private IResourceHelper m_ResourceHelper;

        private Dictionary<int, UniTaskCompletionSource<IObj>> mObjTask;


        public override void Start()
        {
            base.Start();
            m_ObjInfos = new Dictionary<int, ObjInfo>();
            m_ObjGroups = new Dictionary<string, ObjTypeGroup>();
            m_ObjsBeingLoaded = new HashSet<int>();
            m_ObjsToReleaseOnLoad = new HashSet<int>();
            m_RecycleQueue = new Queue<ObjInfo>();
            mObjTask = new Dictionary<int, UniTaskCompletionSource<IObj>>();

            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccessCallback, LoadAssetFailureCallback);
        }

        public override void Tick(float deltaTime)
        {
            while (m_RecycleQueue.Count > 0)
            {
                var ObjInfo = m_RecycleQueue.Dequeue();
                var Obj = ObjInfo.Obj;
                var ObjGroup = (ObjTypeGroup)Obj.ObjTypeGroup;
                if (ObjGroup == null) continue;

                ObjInfo.Status = Define.ObjStatus.WillRecycle;
                Obj.OnRecycle();
                ObjInfo.Status = Define.ObjStatus.Recycled;
                ObjGroup.UnSpawnObj(Obj);
                // ReferencePool.Release(ObjInfo);
            }

            foreach (var ObjGroup in m_ObjGroups)
                try
                {
                    ObjGroup.Value.Update(deltaTime);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
        }

        public override void End()
        {
            base.End();
            m_ObjInfos.Clear();
            m_ObjGroups.Clear();
            m_ObjsBeingLoaded.Clear();
            m_ObjsToReleaseOnLoad.Clear();
            m_RecycleQueue.Clear();
            mObjTask.Clear();
        }

        /// <summary>
        ///     设置辅助工具
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="objHelper"></param>
        public void SetHelper(IResourceHelper helper, IObjHelper objHelper)
        {
            m_ResourceHelper = helper;
            m_ObjHelper = objHelper;
        }

        #region 实体分类组相关

        /// <summary>
        ///     是否存在实体组。
        /// </summary>
        /// <param name="ObjGroupName">实体组名称。</param>
        /// <returns>是否存在实体组。</returns>
        public bool HasObjGroup(string ObjGroupName)
        {
            return m_ObjGroups.ContainsKey(ObjGroupName);
        }

        /// <summary>
        ///     获取实体组。
        /// </summary>
        /// <param name="ObjGroupName">实体组名称。</param>
        /// <returns>要获取的实体组。</returns>
        public ObjTypeGroup GetObjGroup(string ObjGroupName)
        {
            ObjTypeGroup ObjGroup = null;
            if (m_ObjGroups.TryGetValue(ObjGroupName, out ObjGroup)) return ObjGroup;

            return null;
        }

        /// <summary>
        ///     增加实体组。
        /// </summary>
        /// <param name="ObjGroupName">实体组名称。</param>
        /// <returns>是否增加实体组成功。</returns>
        public bool AddObjGroup(string ObjGroupName, Transform groupNode)
        {
            if (HasObjGroup(ObjGroupName)) return false;

            var group = new GameObject(ObjGroupName);
            group.transform.SetParent(groupNode.transform);
            m_ObjGroups.Add(ObjGroupName, new ObjTypeGroup(ObjGroupName, group.transform));

            return true;
        }

        #endregion

        #region 实体相关-show

        /// <summary>
        ///     获取实体信息。
        /// </summary>
        /// <param name="ObjId">实体编号。</param>
        /// <returns>实体信息。</returns>
        private ObjInfo GetObjInfo(int ObjId)
        {
            ObjInfo ObjInfo = null;
            if (m_ObjInfos.TryGetValue(ObjId, out ObjInfo)) return ObjInfo;

            return null;
        }

        /// <summary>
        ///     是否存在实体。
        /// </summary>
        /// <param name="ObjId">实体编号。</param>
        /// <returns>是否存在实体。</returns>
        public bool HasObj(int ObjId)
        {
            return m_ObjInfos.ContainsKey(ObjId);
        }


        /// <summary>
        ///     获取实体。
        /// </summary>
        /// <param name="ObjId">实体编号。</param>
        /// <returns>要获取的实体。</returns>
        public IObj GetObj(int ObjId)
        {
            var ObjInfo = GetObjInfo(ObjId);
            if (ObjInfo == null) return null;
            return ObjInfo.Obj;
        }

        /// <summary>
        ///     是否正在加载实体。
        /// </summary>
        /// <param name="ObjId">实体编号。</param>
        /// <returns>是否正在加载实体。</returns>
        public bool IsLoadingObj(int ObjId)
        {
            return m_ObjsBeingLoaded.Contains(ObjId);
        }

        /// <summary>
        ///     显示实体。
        /// </summary>
        /// <param name="ObjId">实体编号。</param>
        /// <param name="ObjAssetName">实体资源名称。</param>
        /// <param name="ObjGroupName">实体组名称。</param>
        public void ShowObj<T>(int ObjId, string ObjAssetName, string ObjGroupName) where T : ObjLogic
        {
            ShowObj<T>(ObjId, ObjAssetName, ObjGroupName, null);
        }

        /// <summary>
        ///     显示实体。
        /// </summary>
        /// <param name="ObjId">实体编号。</param>
        /// <param name="ObjAssetName">实体资源名称。</param>
        /// <param name="ObjGroupName">实体组名称。</param>
        /// <param name="userData">用户自定义数据。PT:Obj_Init_Data</param>
        public void ShowObj<LogicT>(int ObjId, string ObjAssetName, string ObjGroupName, IShowObjLogicParams userData)
            where LogicT : ObjLogic
        {
            if (!HasObjGroup(ObjGroupName))
            {
                Debug.LogError($"当前无 {ObjGroupName} 类型的根节点");
                return;
            }

            var ObjGroup = GetObjGroup(ObjGroupName);


            var Obj = ObjGroup.SpawnObj(ObjAssetName);
            if (Obj == null)
            {
                m_ObjsBeingLoaded.Add(ObjId);

                //TODO  pool
                var info = new LoadObjInfo(ObjId, ObjGroup, typeof(LogicT), userData);
                m_ResourceHelper.LoadAssetAsync(ObjAssetName, ObjGroup.Name, m_LoadAssetCallbacks, info);
                return;
            }

            InternalShowObj(ObjId, ObjAssetName, ObjGroup, Obj.gameObject, typeof(LogicT), userData);
        }

        /// <summary>
        ///     使用unitask 模仿同步的方式来写逻辑
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="ObjAssetName"></param>
        /// <param name="ObjGroupName"></param>
        /// <param name="userData"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public UniTask<IObj> ShowObjAwait<T>(int objId, string ObjAssetName, string ObjGroupName,
            IShowObjLogicParams userData) where T : ObjLogic
        {
            var tcs = new UniTaskCompletionSource<IObj>();
            mObjTask.Add(objId, tcs);
            ShowObj<T>(objId, ObjAssetName, ObjGroupName, userData);
            return tcs.Task;
        }

        /// <summary>
        ///     加载完成之后的初始化
        /// </summary>
        /// <param name="ObjId"></param>
        /// <param name="ObjAssetName"></param>
        /// <param name="ObjGroup"></param>
        /// <param name="ObjInstance"></param>
        /// <param name="logicType"></param>
        /// <param name="userData"></param>
        private IObj InternalShowObj(int ObjId, string ObjAssetName, ObjTypeGroup ObjGroup, Object ObjInstance,
            Type logicType, IShowObjLogicParams userData)
        {
            try
            {
#if UNITY_EDITOR || LOG_ENABLE
                ObjInstance.name = ObjId.ToString();
#endif
                var Obj = m_ObjHelper.CreateObj(ObjInstance, ObjGroup, userData);

                //TODO 复用
                var info = new ObjInfo(Obj);
                m_ObjInfos.Add(ObjId, info);
                info.Status = Define.ObjStatus.WillInit;
                Obj.OnInit(ObjId, ObjAssetName, ObjGroup, logicType);
                info.Status = Define.ObjStatus.Inited;
                ObjGroup.AddObj(Obj);
                info.Status = Define.ObjStatus.WillShow;
                Obj.OnShow(userData);
                info.Status = Define.ObjStatus.Showed;

                EventManager.Instance.RaiseEvent(new CreateObjSuccessEventArgs { ObjId = ObjId, Obj = Obj });
                return Obj;
            }
            catch (Exception exception)
            {
                EventManager.Instance.RaiseEvent(new CreateObjFailedEventArgs { ObjId = ObjId });
            }

            return null;
        }

        #endregion

        #region 实体相关-Remove

        /// <summary>
        ///     隐藏实体。
        /// </summary>
        /// <param name="ObjId">实体编号。</param>
        public void RemoveObj(int ObjId)
        {
            if (IsLoadingObj(ObjId))
            {
                m_ObjsToReleaseOnLoad.Add(ObjId);
                m_ObjsBeingLoaded.Remove(ObjId);
                return;
            }

            var ObjInfo = GetObjInfo(ObjId);
            InternalRemoveObj(ObjInfo);
        }


        /// <summary>
        ///     隐藏所有已加载的实体。
        /// </summary>
        public void RemoveAllLoadedObjs()
        {
            while (m_ObjInfos.Count > 0)
                foreach (var ObjInfo in m_ObjInfos)
                {
                    InternalRemoveObj(ObjInfo.Value);
                    break;
                }
        }

        /// <summary>
        ///     隐藏所有正在加载的实体。
        /// </summary>
        public void RemoveAllLoadingObjs()
        {
            foreach (var ObjBeingLoaded in m_ObjsBeingLoaded) m_ObjsToReleaseOnLoad.Add(ObjBeingLoaded);

            m_ObjsBeingLoaded.Clear();
        }

        private void InternalRemoveObj(ObjInfo ObjInfo)
        {
            while (ObjInfo.ChildObjCount > 0)
            {
                var childObj = ObjInfo.GetChildObj();
                RemoveObj(childObj.Id);
            }

            if (ObjInfo.Status == Define.ObjStatus.Removed) return;

            var Obj = ObjInfo.Obj;
            DetachObj(Obj.Id);
            ObjInfo.Status = Define.ObjStatus.WillRemove;
            Obj.OnRemove();
            ObjInfo.Status = Define.ObjStatus.Removed;

            var ObjGroup = (ObjTypeGroup)Obj.ObjTypeGroup;
            ObjGroup.RemoveObj(Obj);

            m_ObjInfos.Remove(Obj.Id);
            m_RecycleQueue.Enqueue(ObjInfo);
        }

        #endregion

        #region 实体相关-关联

        /// <summary>
        ///     获取父实体。
        /// </summary>
        /// <param name="childObjId">要获取父实体的子实体的实体编号。</param>
        /// <returns>子实体的父实体。</returns>
        public IObj GetParentObj(int childObjId)
        {
            var childObjInfo = GetObjInfo(childObjId);
            if (childObjInfo == null) return null;

            return childObjInfo.ParentObj;
        }

        /// <summary>
        ///     获取父实体。
        /// </summary>
        /// <param name="childObj">要获取父实体的子实体。</param>
        /// <returns>子实体的父实体。</returns>
        public IObj GetParentObj(IObj childObj)
        {
            if (childObj == null) return null;

            return GetParentObj(childObj.Id);
        }

        /// <summary>
        ///     获取子实体数量。
        /// </summary>
        /// <param name="parentObjId">要获取子实体数量的父实体的实体编号。</param>
        /// <returns>子实体数量。</returns>
        public int GetChildObjCount(int parentObjId)
        {
            var parentObjInfo = GetObjInfo(parentObjId);
            if (parentObjInfo == null) return 0;

            return parentObjInfo.ChildObjCount;
        }

        /// <summary>
        ///     获取子实体。
        /// </summary>
        /// <param name="parentObjId">要获取子实体的父实体的实体编号。</param>
        /// <returns>子实体。</returns>
        public IObj GetChildObj(int parentObjId)
        {
            var parentObjInfo = GetObjInfo(parentObjId);
            if (parentObjInfo == null) return null;

            return parentObjInfo.GetChildObj();
        }

        /// <summary>
        ///     获取子实体。
        /// </summary>
        /// <param name="parentObj">要获取子实体的父实体。</param>
        /// <returns>子实体。</returns>
        public IObj GetChildObj(IObj parentObj)
        {
            if (parentObj == null) return null;

            return GetChildObj(parentObj.Id);
        }

        /// <summary>
        ///     获取所有子实体。
        /// </summary>
        /// <param name="parentObjId">要获取所有子实体的父实体的实体编号。</param>
        /// <returns>所有子实体。</returns>
        public IObj[] GetChildObjs(int parentObjId)
        {
            var parentObjInfo = GetObjInfo(parentObjId);
            if (parentObjInfo == null) return null;

            return parentObjInfo.GetChildObjs();
        }

        /// <summary>
        ///     获取所有子实体。
        /// </summary>
        /// <param name="parentObjId">要获取所有子实体的父实体的实体编号。</param>
        /// <param name="results">所有子实体。</param>
        public void GetChildObjs(int parentObjId, List<IObj> results)
        {
            var parentObjInfo = GetObjInfo(parentObjId);
            if (parentObjInfo == null) return;

            parentObjInfo.GetChildObjs(results);
        }

        /// <summary>
        ///     获取所有子实体。
        /// </summary>
        /// <param name="parentObj">要获取所有子实体的父实体。</param>
        /// <returns>所有子实体。</returns>
        public IObj[] GetChildObjs(IObj parentObj)
        {
            if (parentObj == null) return null;

            return GetChildObjs(parentObj.Id);
        }

        /// <summary>
        ///     获取所有子实体。
        /// </summary>
        /// <param name="parentObj">要获取所有子实体的父实体。</param>
        /// <param name="results">所有子实体。</param>
        public void GetChildObjs(IObj parentObj, List<IObj> results)
        {
            if (parentObj == null) return;

            GetChildObjs(parentObj.Id, results);
        }

        /// <summary>
        ///     附加子实体。
        /// </summary>
        /// <param name="childObjId">要附加的子实体的实体编号。</param>
        /// <param name="parentObjId">被附加的父实体的实体编号。</param>
        public void AttachObj(int childObjId, int parentObjId)
        {
            if (childObjId == parentObjId) return;

            var childObjInfo = GetObjInfo(childObjId);
            if (childObjInfo == null) return;

            if (childObjInfo.Status >= Define.ObjStatus.WillRemove) return;

            var parentObjInfo = GetObjInfo(parentObjId);
            if (parentObjInfo == null) return;

            if (parentObjInfo.Status >= Define.ObjStatus.WillRemove) return;

            var childObj = childObjInfo.Obj;
            var parentObj = parentObjInfo.Obj;
            DetachObj(childObj.Id);
            childObjInfo.ParentObj = parentObj;
            parentObjInfo.AddChildObj(childObj);
            parentObj.OnAttached(childObj);
            childObj.OnAttachTo(parentObj);
        }

        /// <summary>
        ///     附加子实体。
        /// </summary>
        /// <param name="childObjId">要附加的子实体的实体编号。</param>
        /// <param name="parentObj">被附加的父实体。</param>
        public void AttachObj(int childObjId, IObj parentObj)
        {
            if (parentObj == null) return;

            AttachObj(childObjId, parentObj.Id);
        }

        /// <summary>
        ///     附加子实体。
        /// </summary>
        /// <param name="childObj">要附加的子实体。</param>
        /// <param name="parentObjId">被附加的父实体的实体编号。</param>
        public void AttachObj(IObj childObj, int parentObjId)
        {
            if (childObj == null) return;

            AttachObj(childObj.Id, parentObjId);
        }

        /// <summary>
        ///     附加子实体。
        /// </summary>
        /// <param name="childObj">要附加的子实体。</param>
        /// <param name="parentObj">被附加的父实体。</param>
        public void AttachObj(IObj childObj, IObj parentObj)
        {
            if (childObj == null) return;

            if (parentObj == null) return;

            AttachObj(childObj.Id, parentObj.Id);
        }

        /// <summary>
        ///     解除子实体。
        /// </summary>
        /// <param name="childObjId">要解除的子实体的实体编号。</param>
        public void DetachObj(int childObjId)
        {
            var childObjInfo = GetObjInfo(childObjId);
            if (childObjInfo == null) return;

            var parentObj = childObjInfo.ParentObj;
            if (parentObj == null) return;

            var parentObjInfo = GetObjInfo(parentObj.Id);
            if (parentObjInfo == null) return;

            var childObj = childObjInfo.Obj;
            childObjInfo.ParentObj = null;
            parentObjInfo.RemoveChildObj(childObj);
            parentObj.OnDetached(childObj);
            childObj.OnDetachFrom(parentObj);
        }

        /// <summary>
        ///     解除子实体。
        /// </summary>
        /// <param name="childObj">要解除的子实体。</param>
        public void DetachObj(IObj childObj)
        {
            if (childObj == null) return;

            DetachObj(childObj.Id);
        }

        /// <summary>
        ///     解除所有子实体。
        /// </summary>
        /// <param name="parentObjId">被解除的父实体的实体编号。</param>
        public void DetachChildObjs(int parentObjId)
        {
            var parentObjInfo = GetObjInfo(parentObjId);
            if (parentObjInfo == null) return;

            while (parentObjInfo.ChildObjCount > 0)
            {
                var childObj = parentObjInfo.GetChildObj();
                DetachObj(childObj.Id);
            }
        }

        /// <summary>
        ///     解除所有子实体。
        /// </summary>
        /// <param name="parentObj">被解除的父实体。</param>
        public void DetachChildObjs(IObj parentObj)
        {
            if (parentObj == null) return;

            DetachChildObjs(parentObj.Id);
        }

        #endregion

        #region 资源加载回调

        private void LoadAssetSuccessCallback(string ObjAssetName, Object ObjAsset, float duration, object userData)
        {
            var showObjInfo = (LoadObjInfo)userData;

            if (m_ObjsToReleaseOnLoad.Contains(showObjInfo.ObjId))
            {
                m_ObjsToReleaseOnLoad.Remove(showObjInfo.ObjId);
                m_ObjHelper.ReleaseObj(ObjAsset, null);
                return;
            }

            m_ObjsBeingLoaded.Remove(showObjInfo.ObjId);
            var obj = InternalShowObj(showObjInfo.ObjId, ObjAssetName, showObjInfo.ObjGroup,
                m_ObjHelper.InstantiateObj(ObjAsset),
                showObjInfo.LogicType, showObjInfo.UserData);
            //    ReferencePool.Release(showObjInfo);

            //unitask 的加载监听
            mObjTask.TryGetValue(showObjInfo.ObjId, out var tcs);
            if (tcs != null)
            {
                tcs.TrySetResult(obj);
                mObjTask.Remove(showObjInfo.ObjId);
            }
        }

        private void LoadAssetFailureCallback(string ObjAssetName, string errorMessage, object userData)
        {
            var showObjInfo = (LoadObjInfo)userData;

            if (m_ObjsToReleaseOnLoad.Contains(showObjInfo.ObjId))
            {
                m_ObjsToReleaseOnLoad.Remove(showObjInfo.ObjId);
                return;
            }

            m_ObjsBeingLoaded.Remove(showObjInfo.ObjId);
            var appendErrorMessage = string.Format("Load Obj failure, asset name '{0}', error message '{1}'.",
                ObjAssetName, errorMessage);
            EventManager.Instance.RaiseEvent(new CreateObjFailedEventArgs { ObjId = showObjInfo.ObjId });

            //unitask 的加载监听
            mObjTask.TryGetValue(showObjInfo.ObjId, out var tcs);
            if (tcs != null)
            {
                tcs.TrySetException(new Exception(appendErrorMessage));
                mObjTask.Remove(showObjInfo.ObjId);
            }
        }

        #endregion
    }
}