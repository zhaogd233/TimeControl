namespace GameDefine
{
    /// <summary>
    /// 只管大流程
    /// </summary>
        public enum EGameState
        {
            NONE = 0,
            INITIALIZED,                  // 初始化程序
            LOGIN,                        // 登录界面
            CONNECTSERVER,                // 连接服务器
            LOGINROLE,                    // login 角色
            CREATEROLE,                   // 创角
            SELECTROLE,                   //选角
            ENTERWORLDLOADING,            //进入场景loading
            HOME,                         //基地
            GAMEWORLD,                   //进入大世界
        }

    /// <summary>
    /// 子状态机，EnterWorld 状态下激活
    /// </summary>
        public enum EWorldState
        {
            NONE = 0,
            HOME,                         //基地
           
            BATTLE,                       //副本玩法
        }
}