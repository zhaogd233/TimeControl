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
        void Rewind(int rate);

        /// <summary>
        ///     时间操控结束
        /// </summary>
        /// <summary>
        /// </summary>
        void FinishTimeControl();

        #region 生命周期，对外暴漏的可调用接口

        void Initialized(int maxSecond, float updateDelta);

        /*void OnEnable();
        void OnDisable();*/

        void FixedTick(float deltaTime);

        /// <summary>
        ///     销毁先隐藏，等过了记录周期之后，才彻底销毁
        /// </summary>
        void OnDestroy();

        #endregion
    }
}