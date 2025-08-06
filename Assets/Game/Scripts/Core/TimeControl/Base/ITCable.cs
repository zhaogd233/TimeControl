using System;
using UnityEngine;

namespace TVA
{
    public interface ITCable
    {
        /// <summary>
        ///     正播,即便是从回溯到之前再正播，也是重新记录新的，不用走旧的buffer
        ///     相当于只支持单方向回溯，只能自由预览
        /// </summary>
        /// <param name="delaTime"></param>
        /// <param name="rate"></param>
        void Forward(int rate);

        /// <summary>
        ///     只要往回走就是回溯
        ///     不支持自由预览数据。回溯再正播算重新计算，不是用的历史数据
        ///     因为可能回溯之后，新变化value
        /// </summary>
        /// <param name="偏移回溯0代表没回溯 ">♾️->0</param>
        /// <param name="rate"></param>
        /// <param name="rewindHeadRecordAction">rewind 到记录头，可能还是rewind结束，比如子弹记录了3s,0saction就应该销毁</param>
        void Rewind(int rate,Action rewindHeadRecordAction);

        /// <summary>
        ///     时间操控结束
        /// </summary>
        /// <summary>
        /// </summary>
        void FinishTimeControl();

        #region 生命周期，对外暴漏的可调用接口

        void Initialized(int maxSecond, float updateDelta,int maxRate);

        /*void OnEnable();
        void OnDisable();*/

        void FixedTick(float deltaTime);

        /// <summary>
        ///     销毁先隐藏，等过了记录周期之后，
        /// </summary>
        /// <param name="onComplete">注册彻底销毁时的回调</param>
        void FakeDestroy(Action onComplete = null);
        
        /// <summary>
        /// 直接销毁不记录数据用于回溯
        /// </summary>
        void DestroyImmediate(Action onComplete = null);

        #endregion
    }
}