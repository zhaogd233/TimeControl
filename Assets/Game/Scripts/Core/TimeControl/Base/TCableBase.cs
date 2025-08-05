using System;
using UnityEngine;

namespace TVA
{
    public enum Direct
    {
        Forward, //正向
        Rewind, //回溯
        FinishForward //操控结束开始正向
    }

    public abstract class TCableBase<T> : MonoBehaviour, ITCable
    {
        public bool bDebug;

        [HideInInspector] public Direct TCDirect = Direct.Forward;

        private float _destoryTime;

        private float _escapeTime;

        /// <summary>
        ///     已回溯时间
        /// </summary>
        private float _lastRewindSeconds;

        private float _maxSecond;
        private TVRingBuffer<T> _recordbuffer;

        private Action DestoryCompeletyAction;
        private int forwardRate = 1;
        private int rewindRate = 1;
        public bool IsDestorying { private set; get; } //标记已被逻辑销毁，但可能会回溯出来

        protected virtual void Start()
        {
            InitTCObj();
            SetDebug(bDebug);
        }

        public void FixedTick(float deltaTime)
        {
            /*if (TCDirect == Direct.FinishForward)
            {
                FinishTimeControlInternal();
            }*/
            if (TCDirect == Direct.Forward)
            {
                if (IsDestorying)
                {
                    if (_destoryTime > _maxSecond)
                        DestoryCompeletyInternal();
                    else
                        _destoryTime += deltaTime * forwardRate;
                }
                else
                {
                    ForwardInternal(deltaTime);
                }
            }
            else
            {
                if (IsDestorying)
                {
                    if (_destoryTime > 0)
                    {
                        _destoryTime -= deltaTime * rewindRate;
                    }
                    else
                    {
                        IsDestorying = false;
                        _destoryTime = 0;
                    }
                }
                else
                {
                    RewindInternal(deltaTime);
                }
            }
        }

        public void Forward(int rate)
        {
            TCDirect = Direct.Forward;
            forwardRate = Mathf.Max(rate, forwardRate);
        }

        /// <summary>
        ///     回溯&加速
        ///     往回播是回溯，从已有的时间加速还是算回溯
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="rate"></param>
        public void Rewind(int rate)
        {
            TCDirect = Direct.Rewind;
            _lastRewindSeconds = 0;
            rewindRate = rate;
        }

        public void FinishTimeControl()
        {
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                return;
            }

            if (TCDirect == Direct.Rewind)
            {
                TCDirect = Direct.Forward;

                //回溯结束，通知上层逻辑继续执行逻辑运算
                if (!IsDestorying && TryGetRecordValue(_lastRewindSeconds, out var valuesToRead))
                    FinishRewindAction(valuesToRead);
                _recordbuffer.MoveLastBufferPos(_lastRewindSeconds);
                _escapeTime -= _lastRewindSeconds;
                _escapeTime = Mathf.Clamp(_escapeTime, 0, _maxSecond);
                _lastRewindSeconds = 0;
                rewindRate = 1;
            }
            else
            {
                forwardRate = 1;
                TCDirect = Direct.Forward;
            }
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

        /// <summary>
        ///     销毁的时候，先disable,等过了记录周期之后，才彻底销毁
        /// </summary>
        public void FakeDestroy(Action OnComplete = null)
        {
            DestoryCompeletyAction = OnComplete;
            IsDestorying = true;
            _destoryTime = 0;
        }

        /// <summary>
        ///     正播&加速
        ///     只有从没有的数据开始播，才算记录，旧的加速还是回溯。
        ///     rate 倍速要写多份
        /// </summary>
        private void ForwardInternal(float delaTime)
        {
            _escapeTime += delaTime * forwardRate;
            _escapeTime = Mathf.Clamp(_escapeTime, 0, _maxSecond);
            var value = GetCurTrackData(forwardRate);
            for (var i = 0; i < forwardRate; i++) RecordValue(value);
            //判断是否超过已经最大记录，如果
            // TrackAction(rate);
        }

        private void RewindInternal(float deltaTime)
        {
            var offsetRewindSeconds = _lastRewindSeconds + deltaTime * rewindRate;
            if (offsetRewindSeconds < 0 || offsetRewindSeconds > _escapeTime)
                return;

            _lastRewindSeconds = offsetRewindSeconds;

            //
            if (TryGetRecordValue(offsetRewindSeconds, out var valuesToRead))
                RewindAction(valuesToRead);

            //更新写入位置，之后的数据重新写入
            //    buffers.MoveLastBufferPos(seconds);
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

        public void DestoryCompeletyInternal()
        {
            if (_recordbuffer == null)
            {
                Debug.LogError("尚未调用Initialized");
                return;
            }

            TCManager.Instance.RemoveObjectForTracking(this);

            if (DestoryCompeletyAction != null)
                DestoryCompeletyAction();

            _recordbuffer.Clear();
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

        public float GetDestroyingTime()
        {
            return _destoryTime;
        }

        public bool IsTimeControling()
        {
            return rewindRate > 1 || forwardRate > 1;
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
        ///     通知上层最后rewind的状态
        /// </summary>
        /// <param name="rewindValue"></param>
        protected abstract void FinishRewindAction(T rewindValue);

        #endregion
    }
}