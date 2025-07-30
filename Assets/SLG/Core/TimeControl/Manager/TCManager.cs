using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    public class TCManager : MonoBehaviour
    {
        [SerializeField] public int TrackTime;

        private List<ITCable> _TCables; // 时间可操控对象
        
        float rewindSeconds = 0;
        
        /// <summary>
        /// This property returns how many seconds are currently available for rewind
        /// </summary>
        public float HowManySecondsAvailableForRewind { get; private set; }
        
        /// <summary>
        /// Singleton instance of RewindManager
        /// </summary>
        public static TCManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(Instance);
            }

            Instance = this;
        }
        /// <summary>
        /// Lets you add object to track pool even when game is already running.
        /// </summary>
        /// <param name="objectToRewind"></param>
        /// <param name="automaticDestroy">When you rewind before this object is added for tracking, you can choose if object gets disabled or is detroyed when resuming the game.</param>
        public void AddObjectForTracking(ITCable tcable)
        {
            _TCables.Add(tcable);
        }

        public void RemoveObjectForTracking(ITCable tcable)
        {
            _TCables.Remove(tcable);
        }
        
        /// <summary>
        /// Tells you if scene is currently being rewinded
        /// </summary>
        public bool IsBeingRewinded { get; private set; } = false;
        
        public void StartRewindTimeBySeconds(float seconds)
        {
            if (IsBeingRewinded)
                Debug.LogError("The previous rewind must be stopped by calling StopRewindTimeBySeconds() before you start another rewind");
        
            CheckReachingOutOfBounds(seconds);

            rewindSeconds = seconds;
            IsBeingRewinded = true;
        }
        
        public void StopRewindTimeBySeconds()
        {
            if (!IsBeingRewinded)
                Debug.LogError("Rewind must be started before you try to stop it. StartRewindTimeBySeconds() must be called first");

            HowManySecondsAvailableForRewind -= rewindSeconds;
            IsBeingRewinded = false;
        }
        
        private void CheckReachingOutOfBounds(float seconds)
        {
            if (Mathf.Round(seconds*100) > Mathf.Round(HowManySecondsAvailableForRewind*100))
            {
                Debug.LogError("Not enough stored tracked value!!! Reaching on wrong index. Called rewind should be less than HowManySecondsAvailableForRewind property");
                return;
            }
            if (seconds < 0)
            {
                Debug.LogError("Parameter in StartRewindTimeBySeconds() must have positive value!!!");
                return;
            }
        }
        
         private  void FixedUpdate()
            {   
                if (IsBeingRewinded)
                {
                    _TCables.ForEach(x => x.Rewind(rewindSeconds,1f));
                }
                else 
                {
                    _TCables.ForEach(x => x.Forward(1f));
                 
                    //if(TrackingEnabled)
                      //  HowManySecondsAvailableForRewind = Mathf.Min(HowManySecondsAvailableForRewind + Time.fixedDeltaTime, HowManySecondsToTrack);
                }
            }
    }
}