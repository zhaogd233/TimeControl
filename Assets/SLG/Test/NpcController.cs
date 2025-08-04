using System.Collections;
using TMPro;
using TVA;
using UnityEngine;
using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour, IAreaEntityListener
{
    [Header("Animation 组件")] public Animation anim;

    [Header("跑步速度")] public float runSpeed = 2f;

    [Header("可活动区域（中心点）")] public Transform areaCenterPos;

    [Header("区域半径（100x100）")] public float areaHalfSize = 50f;

    [Header("转向速度（度/秒）")] public float turnSpeed = 180f; // 每秒旋转 180°

    public Transform wuqiEffect;

    public TextMeshPro TimeTMP;

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

    private AnimationTCable _animationTCable;

    private bool bUpdateTRS;
    private bool isRunning;

    // 平滑转向目标
    private Quaternion targetRotation;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        if (anim == null)
            anim = GetComponent<Animation>();
        _TCables = GetComponentsInChildren<ITCable>();

        _animationTCable = GetComponent<AnimationTCable>();
        targetRotation = transform.rotation;
        bUpdateTRS = true;
        PlayRandomAnimation();
    }

    private void Update()
    {
        if (!bUpdateTRS)
            return;
        // 平滑旋转
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime * _rate);

        if (isRunning)
        {
            // 持续向前移动
            transform.position += transform.forward * runSpeed * Time.deltaTime * _rate;
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

    private void LateUpdate()
    {
        if (_camera != null && TimeTMP != null)
        {
            // 直接面向相机
            TimeTMP.transform.forward = _camera.transform.forward;

            var recordTime = Mathf.Max(0, _animationTCable.GetRecordTime());
            TimeTMP.text = $"{recordTime:F1}s";

            if (recordTime <= 0)
                TimeTMP.color = Color.red;
            else if (_animationTCable.TCDirect == Direct.Rewind)
                TimeTMP.color = new Color(0.4845996f, 0, 1, 1);
            else if (_animationTCable.IsTimeControling())
                TimeTMP.color = Color.green;
            else
                TimeTMP.color = Color.white;
        }
    }

    private void PlayRandomAnimation(string lastClip = "", float leftTime = 0f)
    {
        StopAllCoroutines();

        if (animationNames == null || animationNames.Length == 0) return;
        if (_animationTCable != null && _animationTCable.TCDirect == Direct.Rewind)
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
        if (_animationTCable != null && _animationTCable.TCDirect == Direct.Forward)
        {
            anim.Play(clipName);
            state.speed = _rate;
        }

        // 如果是 skill 动画，则播放特效
        if (clipName.Contains("Skill_Huixuanzhan") && wuqiEffect != null)
            //   wuqiEffect.Play();
        {
            wuqiEffect.gameObject.SetActive(true);
            var ps = wuqiEffect.GetComponentsInChildren<ParticleSystem>();
            foreach (var particleSystem in ps)
            {
                var main = particleSystem.main;
                main.simulationSpeed = _rate;
            }
        }

        else
            //  wuqiEffect.Stop();
        {
            wuqiEffect.gameObject.SetActive(false);
        }

        if (clipName.Equals("Attack_2")) TryDestory();

        // 延迟到动画结束时调用下一次
        StartCoroutine(PlayNextAfter((state.length - state.time % state.length) / _rate));
    }

    private IEnumerator PlayNextAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayRandomAnimation();
    }

    private void TryDestory()
    {
        foreach (var tCable in _TCables) tCable.OnDestroy();
    }

    #region 受到操控区域的作用

    private ITCable[] _TCables;
    private Camera _camera;
    private int _rate = 1;

    public void OnEnterTCArea(Direct direct, int rate)
    {
        foreach (var tCable in _TCables)
            if (direct == Direct.Rewind)
                tCable.Rewind(rate);
            else
                tCable.Forward(rate);

        _rate = rate;
        //当前播放的需要加速
        if (direct == Direct.Forward)
        {
            if (wuqiEffect.gameObject.activeInHierarchy)
            {
                var ps = wuqiEffect.GetComponentsInChildren<ParticleSystem>();
                foreach (var particleSystem in ps)
                {
                    var main = particleSystem.main;
                    main.simulationSpeed = rate;
                }
            }

            foreach (AnimationState state in anim)
                if (anim.IsPlaying(state.name))
                {
                    anim[state.name].speed = rate;

                    StopAllCoroutines();
                    StartCoroutine(PlayNextAfter((state.length - state.time % state.length) / rate));
                }
        }
        else
        {
            StopAllCoroutines();
            anim.Stop();
            bUpdateTRS = false;
        }
    }

    public void OnStayInTCArea(float deltaTime)
    {
    }

    public void OnExitTCArea(Direct direct)
    {
        _rate = 1;
        foreach (var tCable in _TCables) tCable.FinishTimeControl();

        //当前播放的需要加速
        if (direct == Direct.Forward)
        {
            if (wuqiEffect.gameObject.activeInHierarchy)
            {
                var ps = wuqiEffect.GetComponentsInChildren<ParticleSystem>();
                foreach (var particleSystem in ps)
                {
                    var main = particleSystem.main;
                    main.simulationSpeed = 1;
                }
            }

            foreach (AnimationState state in anim)
                if (anim.IsPlaying(state.name))
                {
                    anim[state.name].speed = 1;

                    StopAllCoroutines();
                    StartCoroutine(PlayNextAfter((state.length - state.time % state.length) / _rate));
                }
        }
        else
        {
            foreach (AnimationState state in anim)
                if (anim.IsPlaying(state.name))
                {
                    anim[state.name].speed = 1;
                    PlayRandomAnimation(state.name, state.time);
                }

            bUpdateTRS = true;
        }
    }

    #endregion
}