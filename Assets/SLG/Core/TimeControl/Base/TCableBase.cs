using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    public abstract class TCableBase<T> : MonoBehaviour,ITCable
    {
        private TVRingBuffer<T> _recordbuffer;

        private float _lastRewindSeconds = 0;

        /// <summary>
        /// 正播&加速
        /// 只有从没有的数据开始播，才算记录，旧的加速还是回溯。
        /// </summary>
        public void Forward(float rate)
        {
           T value = GetCurTrackData(rate);
           RecordValue(value);
            //判断是否超过已经最大记录，如果
           // TrackAction(rate);
        }

        /// <summary>
        /// 回溯&加速
        /// 往回播是回溯，从已有的时间加速还是算回溯
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="rate"></param>
        public void Rewind(float seconds, float rate)
        {
            _lastRewindSeconds = seconds;
            
            //TODO 判断是否超边界，取边界值
            if (TryGetRecordValue(seconds, out T valuesToRead))
                RewindAction(valuesToRead);
            
            //更新写入位置，之后的数据重新写入
        //    buffers.MoveLastBufferPos(seconds);
        }

        public void FinishRewind()
        {
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                return;
            }
            //回溯结束，之后就继续正播
            _recordbuffer.MoveLastBufferPos(_lastRewindSeconds);
            _lastRewindSeconds = 0;
        }

        /// <summary>
        /// 初始化存储数据
        /// </summary>
        public void Initialized(int maxSecond, float updateDelta)
        {
            int countPerSec = (int)(1.0f/updateDelta);
            _recordbuffer = new TVRingBuffer<T>((int)(maxSecond * countPerSec), countPerSec);
            TCManager.Instance.AddObjectForTracking(this);
        }

        /// <summary>
        /// 写入当前值
        /// </summary>
        /// <param name="value"></param>
        public void RecordValue(T value)
        {
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                return;
            }
            _recordbuffer.RecordValue(value);
        }

        public bool TryGetRecordValue(float seconds,out T value)
        {
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                value = default(T);
                return false;
            }

            value = _recordbuffer.ReadValue(seconds);
            return true;
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
            if(_recordbuffer == null)
                Debug.LogError("尚未调用Initialized");
            _recordbuffer.Clear();
            TCManager.Instance.RemoveObjectForTracking(this);
            DestoryCompelety();
        }

        protected virtual void Start()
        {
            InitTCObj();
        }

        #region 子类需要去实现的具体逻辑

        protected abstract void InitTCObj();

        protected abstract T GetCurTrackData(float rate);
        protected abstract void RewindAction(T curValue);
        protected abstract void DestoryCompelety();

        #endregion

    }

}