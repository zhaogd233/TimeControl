using System;

namespace Core.Obj
{
    public class LoadObjInfo
    {
        public LoadObjInfo(int ObjId, ObjTypeGroup ObjGroup, Type logicType, IShowObjLogicParams userData)
        {
            this.ObjId = ObjId;
            this.ObjGroup = ObjGroup;
            UserData = userData;
            LogicType = logicType;
        }

        public int ObjId { get; private set; }

        public ObjTypeGroup ObjGroup { get; private set; }

        public IShowObjLogicParams UserData { get; private set; }

        public Type LogicType { get; private set; }

        public void Clear()
        {
            ObjId = 0;
            ObjGroup = null;
            LogicType = null;
            UserData = null;
        }
    }
}