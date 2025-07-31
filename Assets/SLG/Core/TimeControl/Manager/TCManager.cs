using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    public class TCManager : MonoBehaviour
    {
        [SerializeField] public int TrackTime;

        /// <summary>
        ///     准备结束回溯
        /// </summary>
        private bool _bPrepareFinishRewind;

        private List<ITCable> _TCables; // 时间可操控对象

        private float rewindSeconds;

        /// <summary>
        ///     Singleton instance of RewindManager
        /// </summary>
        public static TCManager Instance { get; private set; }

        /// <summary>
        ///     是否正在回溯
        /// </summary>
        public bool bRewinding { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(Instance);

            _TCables = new List<ITCable>();
            Instance = this;
        }

        private void FixedUpdate()
        {
            if (_bPrepareFinishRewind)
            {
                _bPrepareFinishRewind = false;
                bRewinding = false;

                _TCables.ForEach(x => x.FinishRewind());
            }

            if (bRewinding)
                _TCables.ForEach(x => x.Rewind(rewindSeconds, 1f));
            else
                _TCables.ForEach(x => x.Forward(1f));
        }

        public void StartRewindTimeBySeconds(float seconds)
        {
            if (bRewinding)
                Debug.LogError(
                    "The previous rewind must be stopped by calling StopRewindTimeBySeconds() before you start another rewind");


            rewindSeconds = seconds;
            bRewinding = true;
        }

        /// <summary>
        ///     Call this method to update rewind preview while rewind is active (StartRewindTimeBySeconds() method was called
        ///     before)
        /// </summary>
        /// <param name="seconds">Parameter defining how many seconds should the rewind preview move to (Parameter must be >=0)</param>
        public void SetTimeSecondsInRewind(float seconds)
        {
            rewindSeconds = seconds;
        }

        public void StopRewindTimeBySeconds()
        {
            if (!bRewinding)
                Debug.LogError(
                    "Rewind must be started before you try to stop it. StartRewindTimeBySeconds() must be called first");

            _bPrepareFinishRewind = true;
        }

        #region 操控对象管理

        /// <summary>
        ///     跟踪可被操控的对象
        /// </summary>
        /// <param name="tcable"></param>
        public void AddObjectForTracking(ITCable tcable)
        {
            _TCables.Add(tcable);
        }

        public void RemoveObjectForTracking(ITCable tcable)
        {
            _TCables.Remove(tcable);
        }

        #endregion
    }
}