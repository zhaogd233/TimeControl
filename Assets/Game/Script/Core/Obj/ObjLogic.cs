using UnityEngine;

namespace Core.Obj
{
    /// <summary>
    /// 实体逻辑
    /// </summary>
    public class ObjLogic : MonoBehaviour
    {
        private bool m_Available = false;
        private bool m_Visible = false;
        
        /// <summary>
        /// 对外的实体对象，外部管理对象
        /// </summary>
        private Obj _objActor = null;
        
        //基础信息数据，包括暂存的ObjTransform，以及位置、旋转、缩放等基础信息
        //缓存ObjTransform
        private Transform m_ObjTransform = null;
        private Transform m_OrignTransform = null;
        public Transform ObjTransform
        {
            get
            {
                if(null == m_ObjTransform)
                {
                    m_ObjTransform = transform;
                }
                return m_ObjTransform;
            }
        }
        
        //位置
        public Vector3 Position
        {
            get { return ObjTransform.localPosition; }
            set { ObjTransform.localPosition = value; }
        }

        public Vector3 WorldPosition
        {
            get { return ObjTransform.position; }
            set { ObjTransform.position = value; }
        }
    
        //旋转
        public Quaternion Rotation
        {
            get { return ObjTransform.localRotation; }
            set { ObjTransform.localRotation = value; }
        }
    
        //缩放
        public Vector3 Scale
        {
            get { return ObjTransform.localScale; }
            set { ObjTransform.localScale = value; }
        }
        public virtual void SetScale(float fScale)
        {
            if (null != gameObject)
            {
                ObjTransform.localScale = Vector3.one * fScale;
            }
        }
        
        /// <summary>
        /// 获取实体是否可用。
        /// </summary>
        public bool Available
        {
            get
            {
                return m_Available;
            }
        }

        /// <summary>
        /// 获取或设置实体是否可见。
        /// </summary>
        public bool Visible
        {
            get
            {
                return m_Available && m_Visible;
            }
            set
            {
                if (!m_Available)
                {
                    Debug.LogWarning("Entity '{0}' is not available.", gameObject);
                    return;
                }

                if (m_Visible == value)
                {
                    return;
                }

                m_Visible = value;
                InternalSetVisible(value);
            }
        }

        /// <summary>
        /// 使用对应的参数初始化实体
        /// </summary>
        /// <param name="obj"></param>
        public  virtual void OnInit(Obj obj)
        {
            _objActor = obj;
            m_OrignTransform = ObjTransform;
        }
        
        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnShow(IShowObjLogicParams userData)
        {
            m_Available = true;
            Visible = true;
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        public  virtual void OnRemove()
        {
            Visible = false;
            m_Available = false;
        }
        
        /// <summary>
        /// 实体回收。
        /// </summary>
        public  virtual void OnRecycle()
        {
            Visible = false;
            m_Available = false;
        }

        
        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childObj">附加的子实体。</param>
        public  virtual void OnAttached(IObj childObj)
        {
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childObj">解除的子实体。</param>
        public  virtual void OnDetached(IObj childObj)
        {
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentObj">被附加的父实体。</param>
        /// <param name="parentTransform">被附加父实体的位置。</param>
        public  virtual void OnAttachTo(IObj parentObj, Transform parentTransform)
        {
            ObjTransform.SetParent(parentTransform);
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentObj">被解除的父实体。</param>
        public  virtual void OnDetachFrom(IObj parentObj)
        {
            ObjTransform.SetParent(m_OrignTransform);
        }

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        public virtual void OnUpdate(float elapseSeconds)
        {
        }

        /// <summary>
        /// 设置实体的可见性。
        /// </summary>
        /// <param name="visible">实体的可见性。</param>
        public virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}