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
        protected override void Init(int second,int perSec)
        {
            buffers = new TVRingBuffer<TransformValues>(second * perSec, perSec);
            _transform = transform;
        }

        /*public override void OnEnable()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDisable()
        {
            throw new System.NotImplementedException();
        }*/

      

        protected override void DestoryCompelety()
        {
           // throw new System.NotImplementedException();
           buffers.Clear();
        }

        protected override void TrackAction(float rate)
        {
            TransformValues valuesToWrite;
            valuesToWrite.position = _transform.position;
            valuesToWrite.rotation = _transform.rotation;
            valuesToWrite.scale = _transform.localScale;
            buffers.RecordValue(valuesToWrite);
        }

        protected override void RewindAction(float seconds, float rate)
        {
            TransformValues valuesToRead = buffers.ReadValue(seconds);
            transform.SetPositionAndRotation(valuesToRead.position, valuesToRead.rotation);
            transform.localScale= valuesToRead.scale;
        }
    }
}