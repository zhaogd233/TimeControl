using System.Collections;
using System.Collections.Generic;
using Core.EventBus;
using UnityEngine;

namespace EventMessages
{
  //通知loading 显示变更
  public struct ChangeLoadingTips : IEventMessage
  {
     public string tips;
  }
}
