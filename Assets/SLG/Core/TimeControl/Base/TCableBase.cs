using System;
using UnityEngine;

namespace TVA
{
    public enum Direct
    {
        Forward, //正向
        Rewind, //回溯
    }
    public abstract class TCableBase<T> : MonoBehaviour, ITCable
    {
        private float _lastRewindSeconds,_escapeTime;
        private TVRingBuffer<T> _recordbuffer;
        public bool bDebug = false;

        private float _maxSecond;
        private int forwardRate = 1;
        public Direct TCDirect = Direct.Forward;

        /// <summary>
        ///     回溯结束，通知逻辑层继续更新逻辑，当前记录的最后行为会继续进行，之后逻辑更新
        /// </summary>
        public Action<T, float> FinishRewindEvent;

        /// <summary>
        ///     通知上层逻辑层开始回溯，逻辑内的更新暂停
        /// </summary>
        public Action StartRewindEvent;

        protected virtual void Start()
        {
            InitTCObj();
            SetDebug(bDebug);
        }

        protected void FixedUpdate()
        {
            if(TCDirect == Direct.Forward)
                ForwardInternal(Time.fixedDeltaTime,forwardRate);
            
        }

        public void Forward(int rate)
        {
            this.forwardRate = Mathf.Max(rate,forwardRate);
        }
        
        /// <summary>
        ///     正播&加速
        ///     只有从没有的数据开始播，才算记录，旧的加速还是回溯。
        /// rate 倍速要写多份
        /// </summary>
        public void ForwardInternal(float delaTime,float rate)
        {
            _escapeTime+= delaTime * rate;
            _escapeTime = Mathf.Clamp(_escapeTime, 0, _maxSecond);
            var value = GetCurTrackData(rate);
            for (int i = 0; i < rate; i++)
            {
                RecordValue(value);
            }
            //判断是否超过已经最大记录，如果
            // TrackAction(rate);
        }

        /// <summary>
        ///     回溯&加速
        ///     往回播是回溯，从已有的时间加速还是算回溯
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="rate"></param>
        public void Rewind(float seconds, float rate)
        {
            if (seconds < 0 || seconds > _escapeTime)
            {
                Debug.LogError("越界" + seconds);
                return;
            }
            _lastRewindSeconds = seconds;

            if (TCDirect == Direct.Forward && StartRewindEvent != null)
            {
                TCDirect = Direct.Rewind;
                StartRewindEvent();
            }
            else
            {
                TCDirect = Direct.Rewind;
            }

            //TODO 判断是否超边界，取边界值
            if (TryGetRecordValue(seconds, out var valuesToRead))
                RewindAction(valuesToRead);

            //更新写入位置，之后的数据重新写入
            //    buffers.MoveLastBufferPos(seconds);
        }

        public void RewindOffset(float seconds, float rate)
        {
          //  float targetSec = _escapeTime - seconds;
            Rewind(seconds,rate);
        }

        public void FinishRewind()
        {
            TCDirect = Direct.Forward;
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                return;
            }

            //回溯结束，通知上层逻辑继续执行逻辑运算
            if (TryGetRecordValue(_lastRewindSeconds, out var valuesToRead))
            {
                FinishRewindAction(valuesToRead);

                if (FinishRewindEvent != null)
                    FinishRewindEvent(valuesToRead, _lastRewindSeconds);
            }

            _recordbuffer.MoveLastBufferPos(_lastRewindSeconds);
            _escapeTime -= _lastRewindSeconds;
            _escapeTime = Mathf.Clamp(_escapeTime, 0, _maxSecond);
            _lastRewindSeconds = 0;
        }

        /// <summary>
        ///     初始化存储数据
        /// </summary>
        public void Initialized(int maxSecond, float updateDelta)
        {
            _maxSecond = maxSecond;
            var countPerSec = (int)(1.0f / updateDelta);
            _recordbuffer = new TVRingBuffer<T>(maxSecond * countPerSec, countPerSec);
            TCManager.Instance.AddObjectForTracking(this);
        }

        /*/// <summary>
        /// 生命周期
        /// </summary>
        public abstract void OnEnable();
        public abstract void OnDisable();*/

        /// <summary>
        ///     销毁的时候，先disable,等过了记录周期之后，才彻底销毁
        /// </summary>
        public void OnDestroy()
        {
            //  OnDisable();

            //标记隐藏，下个记录周期之后彻底删除
        }

        /// <summary>
        ///     写入当前值
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

        public bool TryGetRecordValue(float seconds, out T value)
        {
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                value = default;
                return false;
            }

            value = _recordbuffer.ReadValue(seconds);
            return true;
        }

        public void _DestoryCompelety()
        {
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                return;
            }

            _recordbuffer.Clear();
            TCManager.Instance.RemoveObjectForTracking(this);
            DestoryCompelety();
        }

        public void SetDebug(bool b)
        {
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                return;
            }

            _recordbuffer.SetDebug(b);
        }

        public float GetRecordTime()
        {
            return _escapeTime - _lastRewindSeconds;
        }

        #region 子类需要去实现的具体逻辑

        protected abstract void InitTCObj();

        /// <summary>
        ///     获取当前正播记录信息
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        protected abstract T GetCurTrackData(float rate);

        /// <summary>
        ///     根据记录数据回溯当前行为
        /// </summary>
        /// <param name="curValue"></param>
        protected abstract void RewindAction(T curValue);


        /// <summary>
        ///     当回溯结束时刻,根据当前记录继续正播记录剩余的行为
        /// </summary>
        /// <param name="rewindSeconds"></param>
        protected abstract void FinishRewindAction(T rewindValue);

        protected abstract void DestoryCompelety();

        #endregion
    }
}