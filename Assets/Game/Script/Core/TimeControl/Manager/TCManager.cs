using System;
using System.Collections.Generic;
using Core.Base;
using UnityEngine;

namespace Core.TVA
{
    public class TCManager : ModuleManager<TCManager>
    {
        private HashSet<ATCActor> _actors; // 时间可操控对象
        private List<ATCActor> _AddActors; // 时间可操控对象
        private List<ATCActor> _DelActors; // 时间可操控对象
        public int MaxRate = 2;
        public int TrackTime = 5;

        #region manager 生命周期

        public override void Start()
        {
            base.Start();
            _actors = new HashSet<ATCActor>();
            _DelActors = new List<ATCActor>();
            _AddActors = new List<ATCActor>();
        }

        public override void LateUpdate(float deltaTime)
        {
            foreach (var actor in _AddActors) _actors.Add(actor);
            _AddActors.Clear();
            foreach (var actor in _DelActors) _actors.Remove(actor);
            _DelActors.Clear();

            foreach (var actor in _actors)
                try
                {
                    actor.Tick(deltaTime);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
        }

        public override void End()
        {
            base.End();
            _actors.Clear();
            _DelActors.Clear();
            _actors.Clear();
            _actors = null;
            _DelActors = null;
            _actors = null;
        }

        #endregion

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