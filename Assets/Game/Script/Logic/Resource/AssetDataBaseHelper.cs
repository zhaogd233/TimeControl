using Core.Resource;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Game.Script.Logic.Resource
{
    public class AssetDataBaseHelper : IResourceHelper
    {
        public async void LoadAssetAsync(string assetName, string objGroupName, LoadAssetCallbacks callbacks,
            object userData)
        {
            //Add Resmanager task 
            string path = null;

            path = "Assets/Game/Bundle/Test/Model/" + assetName + ".prefab";
            var resObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            // 模拟等待一帧
            await UniTask.Yield();

            if (resObj != null)
            {
                callbacks.LoadAssetSuccessCallback(assetName, resObj, 0, userData);
            }
            else
            {
                Debug.LogError("load asset faild" + path);
                callbacks.LoadAssetFailureCallback(assetName, null, userData);
            }
        }
    }
}