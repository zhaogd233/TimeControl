
namespace Core.NetWork
{
    public class NetPlatformAdapter : INetPlatformAdapter
    {
        //获取IP
#if UNITY_IPHONE && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern string _tryConvIP(string strIP);
#else
        private static string _tryConvIP(string strIP)
        {
            return strIP;
        }
#endif
        public string TryConvIP(string orgIP)
        {
            if (orgIP.Contains(":"))
            {
                return orgIP;
            }

            string retIP = _tryConvIP(orgIP);
            if (string.IsNullOrEmpty(retIP))
            {
                return orgIP;
            }

            return retIP;
        }
    }
}