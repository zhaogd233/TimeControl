using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    public class TCManager : MonoBehaviour
    {
        [SerializeField] public int TrackTime;
        [SerializeField] public int MaxRate = 2;
        private List<ITCable> _AddCables; // 时间可操控对象
        private List<ITCable> _DelCables; // 时间可操控对象

        private HashSet<ITCable> _TCables; // 时间可操控对象

        /// <summary>
        ///     Singleton instance of RewindManager
        /// </summary>
        public static TCManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(Instance);

            _TCables = new HashSet<ITCable>();
            _DelCables = new List<ITCable>();
            _AddCables = new List<ITCable>();
            Instance = this;
        }

        private void FixedUpdate()
        {
            foreach (var tCable in _AddCables) _TCables.Add(tCable);
            _AddCables.Clear();
            foreach (var tCable in _DelCables) _TCables.Remove(tCable);
            _DelCables.Clear();
            foreach (var tCable in _TCables) tCable.FixedTick(Time.fixedDeltaTime);
        }

        #region 操控对象管理

        /// <summary>
        ///     跟踪可被操控的对象
        /// </summary>
        /// <param name="tcable"></param>
        public void AddObjectForTracking(ITCable tcable)
        {
            _AddCables.Add(tcable);
        }

        public void RemoveObjectForTracking(ITCable tcable)
        {
            _DelCables.Add(tcable);
        }

        #endregion
    }
}