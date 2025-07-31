using System.Collections;
using System.Collections.Generic;
using TVA;
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
        ParticleSystem[] _particleSystems;
        float[] _particleTimes;
        protected override void Start()
        {
            base.Start();
            _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
            _particleTimes = new float[_particleSystems.Length];
            for (int i = 0; i < _particleTimes.Length; i++)
            {
                _particleTimes[i] = 0f;
            }

            SetDebug(true);
        }

        protected override void InitTCObj()
        {
            Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime);
        }

        protected override ParticleFrameData GetCurTrackData(float rate)
        {
            ParticleFrameData particleFrameData;
            ParticleTrackedData[] particles = new ParticleTrackedData[_particleSystems.Length];
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                particles[i].isPlaying = _particleSystems[i].isPlaying;
                
                if(_particleSystems[i].isPlaying)
                    _particleTimes[i] += Time.fixedDeltaTime;
                particles[i].particleTime = _particleTimes[i];
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
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                ParticleTrackedData particleTracked = curValue.particles[i];  
                if(particleTracked.isPlaying)
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
            for (int i = 0; i < _particleSystems.Length; i++)
            {
                ParticleTrackedData particleTracked = rewindValue.particles[i];  
                if(particleTracked.isPlaying)
                    _particleSystems[i].Play();
            }
        }

       
        protected override void DestoryCompelety()
        {
            throw new System.NotImplementedException();
        }
    }
}
