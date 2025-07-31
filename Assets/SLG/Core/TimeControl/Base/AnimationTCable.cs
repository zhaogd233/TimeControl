using System;
using UnityEngine;

namespace TVA
{
    public struct LegacyAnimationTrackedData
    {
        public string clipName; // 正在播放的剪辑名
        public float time; // 剪辑内部播放时间（秒）
    }

    public class AnimationTCable : TCableBase<LegacyAnimationTrackedData>
    {
        private Animation anim;

        protected override void Start()
        {
            base.Start();
            anim = GetComponent<Animation>();
        }

        protected override void InitTCObj()
        {
            Initialized(TCManager.Instance.TrackTime, Time.fixedDeltaTime);
          //  SetDebug(true);
        }

        protected override LegacyAnimationTrackedData GetCurTrackData(float rate)
        {
            LegacyAnimationTrackedData data = default;
            // 找到当前正在播放的剪辑（第一个 isPlaying 的剪辑 State）
            foreach (AnimationState state in anim)
                if (state.enabled)
                {
                    data.clipName = state.name;
                    data.time = state.time;
                    break;
                }

            return data;
        }

        protected override void RewindAction(LegacyAnimationTrackedData curValue)
        {
            // 暂停所有播放
            anim.Stop();

            if (!string.IsNullOrEmpty(curValue.clipName))
            {
                // 获取目标剪辑的 state
                var state = anim[curValue.clipName];

                // 确保它被启用
                state.enabled = true;
                // 强制把播放时间设置到你想要 rewind 到的 time
                state.time = curValue.time;

                // 调用 Play，让 Animation 组件知道哪个剪辑当前是活跃的
                anim.Play(curValue.clipName);

                // 最关键的一步：Sample 会立刻把 state.time 对应的 Pose 应用到模型
                anim.Sample();

                // 可选：禁用 state，以免后续被其他 Play 覆盖时产生混乱
                state.enabled = false; // 立刻采样到场景
            }
        }

        protected override void FinishRewindAction(LegacyAnimationTrackedData rewindValue)
        {
            if (!string.IsNullOrEmpty(rewindValue.clipName))
            {
                // 获取目标剪辑的 state
                var state = anim[rewindValue.clipName];

                // 确保它被启用
                state.enabled = true;
                // 强制把播放时间设置到你想要 rewind 到的 time
                state.time = rewindValue.time;
                anim.Play(rewindValue.clipName);
            }
        }

        protected override void DestoryCompelety()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.Play("Attack_1");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.Play("Attack_2");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                anim.Play("Skill_Huixuanzhan");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                anim.Play("Skill_Chuanci_Loop");
            }
        }
    }
}