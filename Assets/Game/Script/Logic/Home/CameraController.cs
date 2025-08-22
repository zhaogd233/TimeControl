using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;             // 跟随的主角
    public float followSmooth = 5f;

    [Header("相机角度控制")]
    public float pitch = 30f;  // 俯仰角
    public float yaw = 0f;     // 偏航角（可选）
    public float distance = 10f; // 相机和目标的距离
    
    [Header("地图边界")]
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float boundaryDamping = 5f;

    [Header("自由视角拖动")]
    public float dragSpeed = 5f;

    [Header("缩放")]
    public float zoomSpeed = 20f;
    public float minFov = 20f;   // 最小 FOV （近）
    public float maxFov = 60f;   // 最大 FOV （远）
    public float followThreshold = 35f; // 小于这个 → 跟随，大于这个 → 自由

    private Vector3 dragOrigin;
    private bool isFreeView = false;
    private Camera cam;

    public Action<bool> OnFreeViewEvent;
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = false; // 确保是透视模式
    }

    void Update()
    {
        HandleZoom();

        if (isFreeView)
            HandleFreeView();
        else
            FollowTarget();
    }

    void FollowTarget()
    {
        if (target == null) return;

        // 用角度计算相机相对偏移
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // 计算期望位置（不跟随 Z）
        /*Vector3 desired = new Vector3(
            target.position.x + followOffset.x,
            target.position.y + followOffset.y,
            transform.position.z // ✅ 保持相机当前 z
        ) + offset;*/
        // 关键：desired 永远基于 target，而不是相机
        Vector3 desired = target.position + offset;
        // 平滑跟随
        Vector3 smoothed = Vector3.Lerp(transform.position, desired, followSmooth * Time.deltaTime);

        // 边界限制
        smoothed.x = Mathf.Clamp(smoothed.x, minBounds.x, maxBounds.x);
        smoothed.y = Mathf.Clamp(smoothed.y, minBounds.y, maxBounds.y);
     
        Vector3 targetPos = smoothed;

// 如果超出边界 → 加一个回弹力
        if (smoothed.x < minBounds.x || smoothed.x > maxBounds.x)
        {
            targetPos.x = Mathf.Clamp(smoothed.x, minBounds.x, maxBounds.x);
            smoothed.x = Mathf.Lerp(smoothed.x, targetPos.x, boundaryDamping * Time.deltaTime);
        }

        if (smoothed.y < minBounds.y || smoothed.y > maxBounds.y)
        {
            targetPos.y = Mathf.Clamp(smoothed.y, minBounds.y, maxBounds.y);
            smoothed.y = Mathf.Lerp(smoothed.y, targetPos.y, boundaryDamping * Time.deltaTime);
        }
        
        transform.position = smoothed;
        transform.LookAt(target); // ✅ 确保始终朝向目标
    }

    void HandleFreeView()
    {
        if (Input.GetMouseButtonDown(0))
            dragOrigin = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 diff = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-diff.x * dragSpeed, -diff.y * dragSpeed, 0);

            transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition;
        }

        // 🔑 边界阻尼
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, minBounds.x, maxBounds.x);
        clamped.y = Mathf.Clamp(clamped.y, minBounds.y, maxBounds.y);

        transform.position = Vector3.Lerp(transform.position, clamped, boundaryDamping * Time.deltaTime);
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newFov = cam.fieldOfView - scroll * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(newFov, minFov, maxFov);

            // 切换模式
           bool curMode = cam.fieldOfView > followThreshold;
            
           if(curMode !=  isFreeView )
           {
               isFreeView = curMode;
                OnFreeViewEvent?.Invoke(isFreeView);
           }
        }
    }
}
