using System.Collections.Generic;

namespace Core.Obj
{
    public sealed partial class ObjManager
    {
        /// <summary>
        /// 实体附加信息。
        /// TODO ♻️
        /// </summary>
        private sealed class ObjInfo
        {
            private IObj m_Obj;
            private Define.ObjStatus m_Status;
            private IObj m_ParentObj;
            private List<IObj> m_ChildObjs;

            public ObjInfo()
            {
                m_Obj = null;
                m_Status = Define.ObjStatus.Unknown;
                m_ParentObj = null;
                m_ChildObjs = new List<IObj>();
            }

            public ObjInfo(IObj obj)
            {
                m_Obj = obj;
                m_Status = Define.ObjStatus.Unknown;
                m_ParentObj = null;
                m_ChildObjs = new List<IObj>();
            }

            public IObj Obj
            {
                get { return m_Obj; }
            }

            public Define.ObjStatus Status
            {
                get { return m_Status; }
                set { m_Status = value; }
            }

            public IObj ParentObj
            {
                get { return m_ParentObj; }
                set { m_ParentObj = value; }
            }

            public int ChildObjCount
            {
                get { return m_ChildObjs.Count; }
            }

            /*public static ObjInfo Create(IObj Obj)
            {
                ObjInfo ObjInfo = ReferencePool.Acquire<ObjInfo>();
                ObjInfo.m_Obj = Obj;
                ObjInfo.m_Status = Define.ObjStatus.WillInit;
                return ObjInfo;
            }*/

            public void Clear()
            {
                m_Obj = null;
                m_Status = Define.ObjStatus.Unknown;
                m_ParentObj = null;
                m_ChildObjs.Clear();
            }

            public IObj GetChildObj()
            {
                return m_ChildObjs.Count > 0 ? m_ChildObjs[0] : null;
            }

            public IObj[] GetChildObjs()
            {
                return m_ChildObjs.ToArray();
            }

            public void GetChildObjs(List<IObj> results)
            {
                if (results == null)
                {
                    return;
                }

                results.Clear();
                foreach (IObj childObj in m_ChildObjs)
                {
                    results.Add(childObj);
                }
            }

            public void AddChildObj(IObj childObj)
            {
                if (m_ChildObjs.Contains(childObj))
                {
                    return;
                }

                m_ChildObjs.Add(childObj);
            }

            public void RemoveChildObj(IObj childObj)
            {
                m_ChildObjs.Remove(childObj);
            }
        }
    }
}