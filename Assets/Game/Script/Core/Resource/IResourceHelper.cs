namespace Core.Resource
{
    public interface IResourceHelper
    {
        /// <summary>
        ///     异步加载资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="objGroupName"></param>
        /// <param name="callbacks"></param>
        /// <param name="userData"></param>
        public void LoadAssetAsync(string assetName, string objGroupName, LoadAssetCallbacks callbacks,
            object userData);
    }
}