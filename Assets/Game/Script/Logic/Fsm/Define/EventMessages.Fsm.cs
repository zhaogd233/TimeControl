using System.Collections;
using System.Collections.Generic;
using Core.EventBus;
using Core.FSM;
using UnityEngine;

namespace EventMessages
{
    /// <summary>
    /// 大世界内，发此事件通知变更游戏内的状态机
    /// </summary>
    public struct ChangeWorldStateMsg : IEventMessage
    {
        public FsmState worldState;
    }
}
