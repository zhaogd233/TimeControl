using UnityEngine;

namespace TVA
{
    /// <summary>
    ///     time control actor
    /// </summary>
    public abstract class ATCActor : MonoBehaviour, IAreaEntityListener
    {
        [Header("实际模型，销毁先隐藏。并保留root的碰撞可见")] public GameObject model;

        private ITCable[] _TCables;

        protected int actorRate = 1;

        private bool bDestroyed;
        private int _pendingCount;

        protected virtual void Start()
        {
            _TCables = GetComponentsInChildren<ITCable>();
        }

        public void OnEnterTCArea(Direct direct, int rate)
        {
            if (bDestroyed)
                return;
            _pendingCount = _TCables.Length;
            foreach (var tCable in _TCables)
                if (direct == Direct.Rewind)
                    tCable.Rewind(rate,OnRewindHeadRecordIntenal);
                else
                    tCable.Forward(rate);

            actorRate = rate;
            //当前播放的需要加速
            if (direct == Direct.Forward)
                BeforeAccelerateAction();
            else
                BeforeRewindAction();
        }


        public void OnExitTCArea(Direct direct)
        {
            if (bDestroyed)
                return;

            actorRate = 1;
            foreach (var tCable in _TCables) tCable.FinishTimeControl();

            //当前播放的需要加速
            if (direct == Direct.Forward)
                AfterAccelerateAction();
            else
                AfterRewindAction();
        }
        
        private void OnRewindHeadRecordIntenal()
        {
            _pendingCount--;

            if (_pendingCount <= 0)
            {
                OnRewindHeadRecord();
            }
        }
        /// <summary>
        ///     销毁实体，实际需要等待
        /// </summary>
        protected  void DestroyActor()
        {
            model?.SetActive(false);
            StopAllActions();
            _pendingCount = _TCables.Length;
            foreach (var tCable in _TCables) tCable.FakeDestroy(DestroyCompelety);
        }

        /// <summary>
        /// 立刻销毁，不再记录无回溯可能
        /// </summary>
        protected void DestroyImmedeletyActor()
        {
            _pendingCount = _TCables.Length;
            foreach (var tCable in _TCables) tCable.DestroyImmediate(DestroyCompelety);
        }

        private void DestroyCompelety()
        {
            bDestroyed = true;
            _pendingCount--;
            
            if (_pendingCount <= 0)
             DestroyImmediate(gameObject);
        }

        #region 需要子类实现的接口

        /// <summary>
        /// 请求销毁时，停止所有行为（实际先隐藏）
        /// </summary>
        protected abstract void StopAllActions();
        
        /// <summary>
        /// 在加速前的处理（动画变速等）
        /// </summary>
        protected abstract void BeforeAccelerateAction();
        
        /// <summary>
        /// 在加速结束的处理（动画恢复正常速度等）
        /// </summary>
        protected abstract void AfterAccelerateAction();
        /// <summary>
        /// 在回溯之前的处理（暂时逻辑的更新）
        /// </summary>
        protected abstract void BeforeRewindAction();
        /// <summary>
        /// 在回溯结束时的处理（恢复更新逻辑）
        /// </summary>
        protected abstract void AfterRewindAction();

        /// <summary>
        /// 当rewind到开始的记录之后的行为，有可能还没结束rewind,如果新创建3s的子弹回溯5s
        /// </summary>
        protected abstract void OnRewindHeadRecord();

        #endregion
    }
}