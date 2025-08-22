using System.Collections;
using System.Collections.Generic;
using Core.EventBus;
using UnityEngine;

namespace EventMessages
{
        #region 登录相关的通知事件

        public struct LoginEventMessage : IEventMessage
        {
            public bool result;
        }

        public struct CreateRoleEventMessage : IEventMessage
        {
            public bool result;
        }

        public struct SelecteRoleEventMessage : IEventMessage
        {
            public bool result;
        }

        #endregion
        
        public struct EnterSceneEventMessage : IEventMessage
        {
            public int sceneId; // 添加场景ID字段
        }
}
