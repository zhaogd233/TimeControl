using System;
using UnityEngine;

namespace Core.Obj
{
    /// <summary>
    /// 外部管理对象，不做逻辑运算。只做管理
    /// </summary>
    public class Obj:MonoBehaviour, IObj
    {
        private int _Id;
        private string _ObjAssetName;
        private IObjTypeGroup _objTypeGroup;
        private ObjLogic _ObjLogic;
        
        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return _Id;
            }
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string AssetName
        {
            get
            {
                return _ObjAssetName;
            }
        }
        
        public IObjTypeGroup ObjTypeGroup
        {
            get { return _objTypeGroup; }
        }

        public ObjLogic Logic
        {
            get
            {
                return _ObjLogic;
            }
        }


        public void OnInit(int ObjId, string ObjName, IObjTypeGroup objTypeGroup,Type logicType)
        {
            _Id = ObjId;
            _ObjAssetName = ObjName;
            _objTypeGroup = objTypeGroup;

            if (_ObjLogic != null)
            {
                if (_ObjLogic.GetType() != logicType)
                {
                    GameObject.Destroy(_ObjLogic);
                    _ObjLogic = null;
                }else
                    _ObjLogic.enabled = true;
            }
            
            if (_ObjLogic == null)
                _ObjLogic = gameObject.AddComponent(logicType) as ObjLogic;
            
            _ObjLogic.OnInit(this);
        }


       public void OnShow(IShowObjLogicParams userData)
        {
            _ObjLogic.OnShow(userData);
        }

        public void OnRemove()
        {
            _ObjLogic.OnRemove();
        }
        
        /// <summary>
        /// TODO 回收
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OnRecycle()
        {
            _ObjLogic.OnRecycle();
            _ObjLogic.enabled = false;
            _Id = 0;
        }

        
        /// <summary>
        /// 当前实体被附加子实体。
        /// </summary>
        /// <param name="childObj">附加的子实体。</param>
        public void OnAttached(IObj childObj)
        {
            _ObjLogic.OnAttached(childObj);
        }

        /// <summary>
        /// 当前实体附加对父实体。
        /// </summary>
        /// <param name="parentObj">附加的实体。</param>
        public void OnAttachTo(IObj parentObj)
        {
            _ObjLogic.OnAttachTo(parentObj,parentObj.Logic.ObjTransform);
        }

        /// <summary>
        /// 当前实体解除父实体。
        /// </summary>
        /// <param name="parentObj">解除的实体。</param>
        public void OnDetachFrom(IObj parentObj)
        {
            _ObjLogic.OnDetachFrom(parentObj);
        }
        
        /// <summary>
        /// 子实体解除当前实体。
        /// </summary>
        /// <param name="childObj">解除的子实体。</param>
        public void OnDetached(IObj childObj)
        {
            _ObjLogic.OnDetachFrom(childObj);
        }

        public void OnUpdate(float elapseSeconds)
        {
            _ObjLogic.OnUpdate(elapseSeconds);
        }
    }
}