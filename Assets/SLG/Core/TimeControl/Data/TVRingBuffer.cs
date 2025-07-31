using UnityEngine;

namespace TVA
{
    /// <summary>
    /// TODO 复用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TVRingBuffer<T>
    {
        T[] _buffer;
        private int _curPos = -1;
        int _capacity;
        int _countPerSec;
        private bool _bDebug;

        public TVRingBuffer(int capacity, int countPerSec)
        {
            _capacity = capacity;
            _countPerSec = countPerSec;
            _buffer = new T[_capacity];
        }
        
        public void RecordValue(T value)
        {
            _curPos++;
            if(_curPos >= _capacity)
                _curPos = 0;
             _buffer[_curPos] = value;
             
             if(_bDebug)
              Debug.LogWarning("写 "+ _curPos);
        }

        public T ReadValue(float seconds)
        {
            if(_bDebug)
             Debug.LogWarning("读 "+ CalculateIndex(seconds) + " seconds: " + seconds + " _curPos: " + _curPos);
            return _buffer[CalculateIndex(seconds)];
        }

        public void MoveLastBufferPos(float seconds)
        {
            _curPos =  CalculateIndex(seconds);    
        }
        private int CalculateIndex(float seconds)
        {
            int howManyBeforeLast = (int)(_countPerSec * (seconds - 0.001));
            int moveBy = _curPos - howManyBeforeLast;
       
            if (moveBy < 0)
            {
                return _capacity + moveBy;
            }
            else
            {
                return _curPos - howManyBeforeLast;
            }
        }

        public void Clear()
        {
            _buffer = null;
        }

        public void SetDebug(bool b)
        {
            _bDebug = b;
        }

    }
}