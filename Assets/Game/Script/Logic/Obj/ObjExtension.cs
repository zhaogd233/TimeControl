using Core.Obj;
using Cysharp.Threading.Tasks;

namespace Logic.Obj
{
    public static class ObjExtension
    {
        public static void ShowObj<T>(this ObjManager objManager,int id, string assetName, GameDefine.ObjType objType, IShowObjLogicParams userData)  where T : ObjLogic
        {
            objManager.ShowObj<T>(id,assetName, objType.ToString(), userData);
        }
        
        public static UniTask<IObj> ShowObjAwait<T>(this ObjManager objManager,int objId,string ObjAssetName, GameDefine.ObjType objType, IShowObjLogicParams userData) where T : ObjLogic
        {
           return objManager.ShowObjAwait<T>(objId, ObjAssetName, objType.ToString(), userData);
        }

    }
}