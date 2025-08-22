using Core.Data;
using Network;
using ProtobufPacket;

namespace Logic.UserData
{
    public class GlobalData : DataBase
    {
        /// <summary>
        /// 服务器同步的时间
        /// </summary>
        public  int ServerAnsiTime = 0;
       

        protected override void InitData()
        {
        }

        protected override void ClearData()
        {
        }
        
    
    }
}