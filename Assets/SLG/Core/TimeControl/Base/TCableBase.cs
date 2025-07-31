using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    public abstract class TCableBase<T> : MonoBehaviour,ITCable
    {
        protected TVRingBuffer<T> buffers;

        /// <summary>
        /// 记录&正播
        /// 只有从没有的数据开始播，才算记录，旧的加速还是回溯。
        /// </summary>
        public void Forward(float rate)
        {
            //判断是否超过已经最大记录，如果
            TrackAction(rate);
        }

        /// <summary>
        /// 回溯
        /// 往回播是回溯，从已有的时间加速还是算回溯
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="rate"></param>
        public void Rewind(float seconds, float rate)
        {
            // TODO 当回溯到born 之前，需要标记为销毁，如果
            RewindAction(seconds, rate);
            
            //更新写入位置，之后的数据重新写入
        //    buffers.MoveLastBufferPos(seconds);
        }

        /// <summary>
        /// 初始化存储数据
        /// </summary>
        public void Initialized(int second, int perSec)
        {
            Init(second,perSec);
            TCManager.Instance.AddObjectForTracking(this);
        }
        /*/// <summary>
        /// 生命周期
        /// </summary>
        public abstract void OnEnable();
        public abstract void OnDisable();*/

        /// <summary>
        /// 销毁的时候，先disable,等过了记录周期之后，才彻底销毁
        /// </summary>
        public void OnDestroy()
        {
          //  OnDisable();
            
            //标记隐藏，下个记录周期之后彻底删除
        }

        public void _DestoryCompelety()
        {
            TCManager.Instance.RemoveObjectForTracking(this);
            DestoryCompelety();
        }

        #region 子类需要去实现的具体逻辑

        protected abstract void Init(int second,int perSec);

        protected abstract void TrackAction(float rate);
        protected abstract void RewindAction(float seconds, float rate);
        protected abstract void DestoryCompelety();

        #endregion

    }

}