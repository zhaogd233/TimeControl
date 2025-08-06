using System.Collections;
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

    private float _escapeTime;
    private float _deadTime;

    private bool bUpdateSelf = true;
    private bool bRewindSelf = false;
    private bool bDestroySelf = false;

    private TransformTCable transformTCable;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _camera = Camera.main;
        transformTCable = GetComponent<TransformTCable>();
    }

    private void Update()
    {
        if (bUpdateSelf)
        {
            _escapeTime += Time.deltaTime * actorRate;
            transform.position += velocity * speed * Time.deltaTime * actorRate;
            
            if(_escapeTime >= LifeTime)
                DestroyActor();
        }
        else if (bRewindSelf)
        {
            //死亡回溯，不处理飞行时长
            if(!transformTCable.IsDestorying)
              _escapeTime -= Time.deltaTime * actorRate;
        }
    }

    private void LateUpdate()
    {
        if (_camera != null && TimeTMP != null)
        {
            // 直接面向相机
            TimeTMP.transform.forward = _camera.transform.forward;

            var recordTime = Mathf.Max(0, transformTCable.GetRecordTime());
            TimeTMP.text = $"{recordTime:F1}s";

            if (transformTCable.IsDestorying)
            {
                TimeTMP.text = $"dead:{transformTCable.GetDestroyingTime():F1}s";
                TimeTMP.color = Color.black;
                return;
            }

            if (recordTime <= 0)
                TimeTMP.color = Color.red;
            else if (transformTCable.TCDirect == Direct.Rewind)
                TimeTMP.color = new Color(0.4845996f, 0, 1, 1);
            else if (transformTCable.IsTimeControling())
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
    /// 创建的子弹直接销毁
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    protected override void OnRewindHeadRecord()
    {
        DestroyImmedeletyActor();
    }
}