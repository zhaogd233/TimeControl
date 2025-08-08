using System;
using System.Collections.Generic;
using UnityEngine;

namespace TVA
{
    /// <summary>
    ///     TODO 复用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TVRingBuffer<T>
    {
        private readonly ulong[] _bitmask; // 每个ulong表示64帧的变化标记
        private readonly int _bitmaskBlockCount;
        private readonly int _capacity;
        private readonly IEqualityComparer<T> _comparer; // 默认比较器
        private readonly int _countPerSec;
        private bool _bDebug;

        private readonly T[] _data;
        private bool _hasFirstValue; // 防止首值和default 一样。

        private T _lastValue;
        private int _logicalFrame = -1;
        private readonly float _maxRate;
        private int _writeIndex = -1;

        public TVRingBuffer(int capacity, int countPerSec, int maxRate, IEqualityComparer<T> comparer)
        {
            _maxRate = maxRate;
            _countPerSec = countPerSec;
            _capacity = (capacity + 63) / 64 * 64; // 向上对齐64
            _data = new T[_capacity];
            _bitmaskBlockCount = _capacity / 64;
            _bitmask = new ulong[_bitmaskBlockCount];
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        /*
        public void RecordValue(T value)
        {
            _frameIndex++;
            if (_frameIndex >= _capacity)
                _frameIndex = 0;
            _data[_frameIndex] = value;

            if (_bDebug)
                Debug.LogWarning("写 " + _frameIndex);
        }
        */

        /// <summary>
        ///     写入新值。如果与上一个值不同，则记录并设置bit。
        /// </summary>
        public void RecordValue(T value)
        {
            _logicalFrame++;

            var block = _logicalFrame / 64 % _bitmaskBlockCount;
            var bit = _logicalFrame % 64;

            var isFirstInBitmaskCycle = _logicalFrame % _capacity == 0; //每次环形回来，强制重写新值，防止全都为0
            if (!_hasFirstValue || isFirstInBitmaskCycle || !_comparer.Equals(value, _lastValue))
            {
                _hasFirstValue = true;
                _writeIndex = (_writeIndex + 1) % _capacity;
                _data[_writeIndex] = value;
                _bitmask[block] |= 1UL << bit;
                _lastValue = value;

                if (_bDebug)
                    Debug.Log($"写入 idx={_writeIndex} 逻辑帧={_logicalFrame} 位图[{block}]第{bit}位=1");
            }
            else
            {
                _bitmask[block] &= ~(1UL << bit);
                if (_bDebug)
                    Debug.Log($"无变化 逻辑帧={_logicalFrame} 位图[{block}]第{bit}位=0");
            }
        }

        /// <summary>
        ///     从当前帧往回查找 frameOffset 帧的数据（0=当前帧，1=上一帧...）
        /// </summary>
        public bool TryGetValue(int targetFrame, out T value)
        {
            if (targetFrame < 0)
            {
                value = default;
                return false;
            }

            for (var f = targetFrame; f >= 0; f--)
            {
                var block = f / 64 % _bitmaskBlockCount;
                var bit = f % 64;
                if ((_bitmask[block] & (1UL << bit)) != 0) // targetFrame 使用的是f帧的数据
                {
                    var changesSinceF = CountSetBitsInRange(f + 1, _logicalFrame); //计算f帧到当前帧，有写入多少次变化数据
                    var dataIndex = (_writeIndex - changesSinceF + _capacity) % _capacity;
                    value = _data[dataIndex];

                    if (_bDebug)
                        Debug.Log($"读取 逻辑帧 {targetFrame} 最近变化帧 {f} 索引 {dataIndex}");

                    return true;
                }
            }

            value = default;
            return false;
        }

        public int GetWriteIndex(int targetFrame)
        {
            if (targetFrame < 0) return -1;

            for (var f = targetFrame; f >= 0; f--)
            {
                var block = f / 64 % _bitmaskBlockCount;
                var bit = f % 64;
                if ((_bitmask[block] & (1UL << bit)) != 0) // targetFrame 使用的是f帧的数据
                {
                    var changesSinceF = CountSetBitsInRange(f + 1, _logicalFrame); //计算f帧到当前帧，有写入多少次变化数据
                    var dataIndex = (_writeIndex - changesSinceF + _capacity) % _capacity;

                    if (_bDebug)
                        Debug.Log($"重置写Index 逻辑帧 {targetFrame} 最近变化帧 {f} 索引 {dataIndex}");

                    return dataIndex;
                }
            }

            return -1;
        }

        private int CountSetBitsInRange(int startFrame, int endFrame)
        {
            var count = 0;
            for (var f = startFrame; f <= endFrame; f++)
            {
                var block = f / 64 % _bitmaskBlockCount;
                var bit = f % 64;
                if ((_bitmask[block] & (1UL << bit)) != 0)
                    count++;
            }

            return count;
        }


        public T ReadValue(float seconds)
        {
            if (TryGetValue(CalculateIndex(seconds), out var value))
                return value;
            Debug.LogError($"读失败 {typeof(T).Name}" + CalculateIndex(seconds) + " seconds: " + seconds + " _curPos: " +
                           _logicalFrame);
            return default;
        }

        public void MoveLastBufferPos(float seconds)
        {
            var targetFrame = CalculateIndex(seconds);
            _writeIndex = GetWriteIndex(targetFrame);
            _logicalFrame = targetFrame;
        }

        private int CalculateIndex(float seconds)
        {
            var howManyBeforeLast = (int)(_countPerSec * (seconds - 0.001) / _maxRate);
            var moveBy = _logicalFrame - howManyBeforeLast;

            if (moveBy < 0) return _capacity + moveBy;

            return _logicalFrame - howManyBeforeLast;
        }

        public void Clear()
        {
            Array.Clear(_bitmask, 0, _bitmask.Length);
            Array.Clear(_data, 0, _data.Length);
            _logicalFrame = -1;
            _lastValue = default;
        }

        public void SetDebug(bool b)
        {
            _bDebug = b;
        }
    }
}