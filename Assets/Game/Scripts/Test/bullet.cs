using System.Collections;
using TMPro;
using TVA;
using UnityEngine;

public class bullet : MonoBehaviour, IAreaEntityListener
{
    [HideInInspector] public Vector3 velocity;

    public float speed = 10f;
    public TextMeshPro TimeTMP;

    public float LifeTime = 5f;
    private Camera _camera;

    private float _escapeTime;

    private int _rate = 1;

    private bool bUpdateSelf = true;

    private TransformTCable transformTCable;

    // Start is called before the first frame update
    private void Start()
    {
        _camera = Camera.main;
        transformTCable = GetComponent<TransformTCable>();
        StartCoroutine(DelayDestroy());
    }

    private void Update()
    {
        if (bUpdateSelf)
        {
            _escapeTime += Time.deltaTime;
            transform.position += velocity * speed * Time.deltaTime * _rate;
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

            ;
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

    public void OnEnterTCArea(Direct direct, int rate)
    {
        StopAllCoroutines();
        if (direct == Direct.Rewind)
        {
            bUpdateSelf = false;
        }
        else
        {
            bUpdateSelf = true;
            _rate = rate;
        }
    }

    public void OnExitTCArea(Direct direct)
    {
        StopAllCoroutines();
        bUpdateSelf = true;
        _rate = 1;
    }

    // Update is called once per frame
    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(LifeTime);
        Destroy(gameObject);
    }
}