using System;
using UnityEngine;

namespace TVA
{
    public struct TransformValues
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
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
            Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime);
        }

        protected override void DestoryCompelety()
        {
            // throw new System.NotImplementedException();
        }

        protected override TransformValues GetCurTrackData(float rate)
        {
            TransformValues valuesToWrite;
            valuesToWrite.position = _transform.position;
            valuesToWrite.rotation = _transform.rotation;
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