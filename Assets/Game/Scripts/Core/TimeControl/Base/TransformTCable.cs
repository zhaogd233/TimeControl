using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    public struct TransformValues
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    public class TransformValuesComparer : IEqualityComparer<TransformValues>
    {
        private readonly float _positionEpsilon;
        private readonly float _rotationEpsilon;
        private readonly float _scaleEpsilon;

        public TransformValuesComparer(float posEps = 0.001f, float rotEps = 0.5f, float scaleEps = 0.001f)
        {
            _positionEpsilon = posEps;
            _rotationEpsilon = rotEps;
            _scaleEpsilon = scaleEps;
        }

        public bool Equals(TransformValues a, TransformValues b)
        {
            return Vector3.Distance(a.position, b.position) < _positionEpsilon &&
                   Quaternion.Angle(a.rotation, b.rotation) < _rotationEpsilon &&
                   Vector3.Distance(a.scale, b.scale) < _scaleEpsilon;
        }

        public int GetHashCode(TransformValues obj)
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 31 + obj.position.GetHashCode();
                hash = hash * 31 + obj.rotation.GetHashCode();
                hash = hash * 31 + obj.scale.GetHashCode();
                return hash;
            }
        }
    }

    public class TransformTCable : TCableBase<TransformValues>
    {
        private Transform _transform;

        protected override void Start()
        {
            base.Start();
            _transform = transform;
        }

        protected override void InitTCObj()
        {
            Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime, TCManager.Instance.MaxRate,
                new TransformValuesComparer());
        }

        protected override void FinishRewindAction(TransformValues rewindValue)
        {
        }

        protected override TransformValues GetCurTrackData(float rate)
        {
            TransformValues valuesToWrite;
            _transform.GetPositionAndRotation(out valuesToWrite.position, out valuesToWrite.rotation);
            valuesToWrite.scale = _transform.localScale;
            return valuesToWrite;
        }

        protected override void RewindAction(TransformValues valuesToRead)
        {
            transform.SetPositionAndRotation(valuesToRead.position, valuesToRead.rotation);
            transform.localScale = valuesToRead.scale;
        }
    }
}