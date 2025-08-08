using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    public class TCManager : MonoBehaviour
    {
        [SerializeField] public int TrackTime;
        [SerializeField] public int MaxRate = 2;

        private HashSet<ATCActor> _actors; // 时间可操控对象
        private List<ATCActor> _AddActors; // 时间可操控对象
        private List<ATCActor> _DelActors; // 时间可操控对象

        /// <summary>
        ///     Singleton instance of RewindManager
        /// </summary>
        public static TCManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(Instance);

            _actors = new HashSet<ATCActor>();
            _DelActors = new List<ATCActor>();
            _AddActors = new List<ATCActor>();
            Instance = this;
        }

        private void FixedUpdate()
        {
            foreach (var actor in _AddActors) _actors.Add(actor);
            _AddActors.Clear();
            foreach (var actor in _DelActors) _actors.Remove(actor);
            _DelActors.Clear();

            var deltaTime = Time.fixedDeltaTime;
            foreach (var actor in _actors) actor.Tick(deltaTime);
        }

        #region 操控对象管理

        /// <summary>
        ///     跟踪可被操控的对象
        /// </summary>
        /// <param name="tcable"></param>
        public void AddObjectForTracking(ATCActor actor)
        {
            _AddActors.Add(actor);
        }

        public void RemoveObjectForTracking(ATCActor actor)
        {
            _DelActors.Add(actor);
        }

        #endregion
    }
}