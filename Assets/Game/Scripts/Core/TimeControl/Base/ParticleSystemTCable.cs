using System;
using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    public struct ParticleTrackedData
    {
        public bool isPlaying;
        public float particleTime;
    }

    public struct ParticleFrameData
    {
        public ParticleTrackedData[] particles;
    }

    public class ParticleFrameDataComparer : IEqualityComparer<ParticleFrameData>
    {
        private readonly float _timeEpsilon;

        public ParticleFrameDataComparer(float timeEpsilon = 0.01f)
        {
            _timeEpsilon = timeEpsilon;
        }

        public bool Equals(ParticleFrameData a, ParticleFrameData b)
        {
            if (a.particles == null && b.particles == null)
                return true;
            if (a.particles == null || b.particles == null)
                return false;
            if (a.particles.Length != b.particles.Length)
                return false;

            for (int i = 0; i < a.particles.Length; i++)
            {
                var pa = a.particles[i];
                var pb = b.particles[i];

                if (pa.isPlaying != pb.isPlaying)
                    return false;
                if (Math.Abs(pa.particleTime - pb.particleTime) > _timeEpsilon)
                    return false;
            }

            return true;
        }

        public int GetHashCode(ParticleFrameData obj)
        {
            if (obj.particles == null)
                return 0;

            int hash = 17;
            foreach (var p in obj.particles)
            {
                hash = hash * 31 + p.isPlaying.GetHashCode();
                hash = hash * 31 + p.particleTime.GetHashCode();
            }

            return hash;
        }
    }

    public class ParticleSystemTCable : TCableBase<ParticleFrameData>
    {
        private ParticleSystem[] _particleSystems;
        private float[] _particleTimes;

        protected override void Start()
        {
            base.Start();
            _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
            _particleTimes = new float[_particleSystems.Length];
            //    SetDebug(true);
        }

        private void OnEnable()
        {
            if (_particleSystems != null)
                for (var i = 0; i < _particleTimes.Length; i++)
                    _particleTimes[i] = 0f;
        }

        protected override void InitTCObj()
        {
            Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime, TCManager.Instance.MaxRate,
                new ParticleFrameDataComparer());
        }

        protected override ParticleFrameData GetCurTrackData(float rate)
        {
            ParticleFrameData particleFrameData;
            var particles = new ParticleTrackedData[_particleSystems.Length];
            for (var i = 0; i < _particleSystems.Length; i++)
            {
                particles[i].isPlaying =
                    _particleSystems[i].isPlaying && _particleSystems[i].gameObject.activeInHierarchy;

                if (_particleSystems[i].isPlaying)
                    _particleTimes[i] += Time.fixedDeltaTime;
                particles[i].particleTime = _particleTimes[i] % _particleSystems[i].main.duration;
            }

            particleFrameData.particles = particles;
            return particleFrameData;
        }

        protected override void RewindAction(ParticleFrameData curValue)
        {
            if (_particleSystems.Length == 0 || curValue.particles == null ||
                curValue.particles.Length != _particleSystems.Length)
            {
                Debug.LogError("RewindAction: Particle System count mismatch!");
                return;
            }

            for (var i = 0; i < _particleSystems.Length; i++)
            {
                var particleTracked = curValue.particles[i];
                if (particleTracked.isPlaying)
                    _particleSystems[i].Simulate(particleTracked.particleTime, false, true, false);
            }
        }

        protected override void FinishRewindAction(ParticleFrameData rewindValue)
        {
            if (_particleSystems.Length == 0 || rewindValue.particles == null ||
                rewindValue.particles.Length != _particleSystems.Length)
            {
                Debug.LogError("RewindAction: Particle System count mismatch!");
                return;
            }

            for (var i = 0; i < _particleSystems.Length; i++)
            {
                var particleTracked = rewindValue.particles[i];
                if (particleTracked.isPlaying)
                {
                    var main = _particleSystems[i].main;
                    main.simulationSpeed = 1;
                    _particleSystems[i].Play();
                }
            }
        }

        protected override bool CheckMainValid()
        {
            throw new NotImplementedException();
        }
    }
}