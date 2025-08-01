using System.Collections;
using TVA;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{
    [Header("Animation 组件")] public Animation anim;

    [Header("跑步速度")] public float runSpeed = 2f;

    [Header("可活动区域（中心点）")] public Transform areaCenterPos;

    [Header("区域半径（100x100）")] public float areaHalfSize = 50f;

    [Header("转向速度（度/秒）")] public float turnSpeed = 180f; // 每秒旋转 180°

    public Transform wuqiEffect;

    private AnimationTCable _animationTCable;

    private readonly string[] animationNames = new string[11]
    {
        "Stand",
        "Skill_Tiaozhan",
        "Skill_Huixuanzhan",
        "Attack_Stand",
        "Attack_Run",
        "Skill_Chuanci_Loop",
        "Skill_Chuanci_End",
        "Skill_Chuanci_Start",
        "Skill_Tiaozhan",
        "Attack_1",
        "Attack_2"
    };

    private bool bUpdateTRS;
    private bool isRunning;

    // 平滑转向目标
    private Quaternion targetRotation;

    private void Start()
    {
        if (anim == null)
            anim = GetComponent<Animation>();
        _animationTCable = GetComponent<AnimationTCable>();
        _animationTCable.StartRewindEvent = OnBeginRewindAnimation;
        _animationTCable.FinishRewindEvent = OnEndRewindAnimation;
        targetRotation = transform.rotation;
        bUpdateTRS = true;
        PlayRandomAnimation();
    }

    private void Update()
    {
        if (!bUpdateTRS)
            return;
        // 平滑旋转
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        if (isRunning)
        {
            // 持续向前移动
            transform.position += transform.forward * runSpeed * Time.deltaTime;
            var needTurn = false;

            // 如果出界 → 掉头
            var pos2D = new Vector2(transform.position.x, transform.position.z);
            if (Mathf.Abs(pos2D.x - areaCenterPos.position.x) > areaHalfSize ||
                Mathf.Abs(pos2D.y - areaCenterPos.position.z) > areaHalfSize)
                needTurn = true;

            if (needTurn)
            {
                // 随机一个大概 90~180° 的新方向
                var newY = transform.eulerAngles.y + Random.Range(90f, 180f);
                targetRotation = Quaternion.Euler(0, newY, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (AnimationState state in anim)
            if (state.enabled)
            {
                // Debug.Log(anim[state.name].name +" "+ anim[state.name].time);
            }
    }

    private void OnBeginRewindAnimation()
    {
        StopCoroutine("PlayNextAfter");
        anim.Stop();

        bUpdateTRS = false;
    }

    /// <summary>
    ///     动画行为已经恢复并继续播放
    /// </summary>
    /// <param name="curAnimData"></param>
    /// <param name="time"></param>
    private void OnEndRewindAnimation(LegacyAnimationTrackedData curAnimData, float time)
    {
        PlayRandomAnimation(curAnimData.clipName, curAnimData.time);
        bUpdateTRS = true;
    }

    private void PlayRandomAnimation(string lastClip = "", float leftTime = 0f)
    {
        if (animationNames == null || animationNames.Length == 0) return;
        if (_animationTCable != null && _animationTCable.bRewinding)
            return;

        // 随机挑选动画
        var clipName = string.IsNullOrEmpty(lastClip)
            ? animationNames[Random.Range(0, animationNames.Length)]
            : lastClip;
        // 获取目标剪辑的 state
        var state = anim[clipName];

        // 确保它被启用
        state.enabled = true;
        // 强制把播放时间设置到你想要 rewind 到的 time
        state.time = leftTime;

        // 判断是否是跑步动画
        isRunning = clipName.ToLower().Contains("run") || clipName.ToLower().Contains("loop");
        //isRunning = true;

        if (!isRunning)
            // 非跑步动画 → 先随机旋转
        {
            // 非跑步动画 → 随机旋转（用平滑过渡）
            var newY = Random.Range(0f, 360f);
            targetRotation = Quaternion.Euler(0, newY, 0);
        }

        // 播放动画
        if (_animationTCable != null && !_animationTCable.bRewinding)
            anim.Play(clipName);

        // 如果是 skill 动画，则播放特效
        if (clipName.Contains("Skill_Huixuanzhan") && wuqiEffect != null)
            //   wuqiEffect.Play();
            wuqiEffect.gameObject.SetActive(true);
        else
            //  wuqiEffect.Stop();
            wuqiEffect.gameObject.SetActive(false);

        // 延迟到动画结束时调用下一次
        StartCoroutine(PlayNextAfter(state.length - state.time % state.length));
    }

    private IEnumerator PlayNextAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayRandomAnimation();
    }
}