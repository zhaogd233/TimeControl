namespace Core.Obj
{
    public class Define
    {
        /// <summary>
        ///     实体状态。
        /// </summary>
        public enum ObjStatus : byte
        {
            Unknown = 0,
            WillInit,
            Inited,
            WillShow,
            Showed,
            WillRemove,
            Removed,
            WillRecycle,
            Recycled
        }
    }
}