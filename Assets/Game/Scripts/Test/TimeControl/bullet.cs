using TMPro;
using TVA;
using UnityEngine;

public class bullet : ATCActor
{
    [HideInInspector] public Vector3 velocity;

    public float speed = 10f;
    public TextMeshPro TimeTMP;

    public float LifeTime = 5f;
    private Camera _camera;
    private float _deadTime;

    private float _escapeTime;
    private bool bRewindSelf;

    private bool bUpdateSelf = true;

    private FPSDisplay fpsDisplay;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _camera = Camera.main;
        fpsDisplay = FindObjectOfType<FPSDisplay>();
    }

    private void Update()
    {
        if (bUpdateSelf)
        {
            _escapeTime += Time.deltaTime * actorRate;
            transform.position += velocity * speed * Time.deltaTime * actorRate;

            if (_escapeTime >= LifeTime)
                DestroyActor();
        }
        else if (bRewindSelf)
        {
            //死亡回溯，不处理飞行时长
            if (MainTCable != null && !MainTCable.IsDestorying)
                _escapeTime -= Time.deltaTime * actorRate;
        }
    }

    private void LateUpdate()
    {
        if (_camera != null && TimeTMP != null && MainTCable != null)
        {
            if (fpsDisplay != null)
            {
                TimeTMP.gameObject.SetActive(fpsDisplay.HUDEnabled);

                if (!fpsDisplay.HUDEnabled)
                    return;
            }

            // 直接面向相机
            TimeTMP.transform.forward = _camera.transform.forward;

            var recordTime = Mathf.Max(0, MainTCable.GetRecordTime());
            TimeTMP.text = $"{recordTime:F1}s";

            if (MainTCable.IsDestorying)
            {
                TimeTMP.text = $"dead:{MainTCable.GetDestroyingTime():F1}s";
                TimeTMP.color = Color.black;
                return;
            }

            if (recordTime <= 0)
                TimeTMP.color = Color.red;
            else if (MainTCable.TCDirect == Direct.Rewind)
                TimeTMP.color = new Color(0.4845996f, 0, 1, 1);
            else if (MainTCable.IsTimeControling())
                TimeTMP.color = Color.green;
            else
                TimeTMP.color = Color.white;
        }
    }

    protected override void StopAllActions()
    {
        bUpdateSelf = false;
    }

    protected override void BeforeAccelerateAction()
    {
        bUpdateSelf = true;
    }

    protected override void AfterAccelerateAction()
    {
        bUpdateSelf = true;
    }

    protected override void BeforeRewindAction()
    {
        bUpdateSelf = false;
        bRewindSelf = true;
    }

    protected override void AfterRewindAction()
    {
        bUpdateSelf = true;
        bRewindSelf = false;
    }

    /// <summary>
    ///     创建的子弹直接销毁
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected override void OnRewindHeadRecord()
    {
        DestroyImmedeletyActor();
    }
}