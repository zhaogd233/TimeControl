using System;
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
            Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime);
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
    }
}