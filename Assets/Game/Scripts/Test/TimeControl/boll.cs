using System.Collections.Generic;
using TVA;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color overlapColor = Color.red;

    /// <summary>
    ///     回溯圈的单位离开之后还能继续回溯。
    ///     加速圈的单位离开之后就不能继续加速
    /// </summary>
    public Direct direct;

    public LayerMask npcLayer; // 只检测 NPC 层
    private readonly List<Ball> overlappingBalls = new();
    private int areaRate = 1;
    private bool beginTC;
    private float checkRadius;

    public HashSet<IAreaEntityListener> npcsInside = new();
    private int overlapCount;
    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = normalColor;

        // 获取模型的包围盒
        var bounds = GetComponent<MeshRenderer>().bounds;

        // 半径取最大边长的一半（保证覆盖整个球）
        checkRadius = GetSphereColliderRadius();
    }

    private void OnDestroy()
    {
        // 通知所有重叠的球：“我走了”
        foreach (var otherBall in overlappingBalls)
            if (otherBall != null)
                otherBall.NotifyBallRemoved(this);
        overlappingBalls.Clear();

        foreach (var entity in npcsInside)
            if (entity != null)
                entity.OnExitTCArea(direct);

        beginTC = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            var otherBall = other.GetComponent<Ball>();
            if (otherBall != null && !overlappingBalls.Contains(otherBall))
            {
                overlappingBalls.Add(otherBall);
                overlapCount++;
                UpdateColor();
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            var entity = other.gameObject.GetComponent<IAreaEntityListener>();
            if (entity != null)
                if (direct == Direct.Forward)
                {
                    if (!npcsInside.Contains(entity))
                        npcsInside.Add(entity);
                    if (beginTC)
                        entity.OnEnterTCArea(direct, areaRate);
                }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            var otherBall = other.GetComponent<Ball>();
            if (otherBall != null && overlappingBalls.Contains(otherBall))
            {
                overlappingBalls.Remove(otherBall);
                overlapCount = Mathf.Max(0, overlapCount - 1);
                UpdateColor();
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("NPC") && direct == Direct.Forward)
        {
            var entity = other.gameObject.GetComponent<IAreaEntityListener>();
            if (entity != null)
            {
                if (beginTC)
                    entity.OnExitTCArea(direct);
                npcsInside.Remove(entity);
            }
        }
    }

    private float GetSphereColliderRadius()
    {
        var sc = GetComponent<SphereCollider>();
        if (sc != null)
            return sc.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        return 0f;
    }

    public void NotifyBallRemoved(Ball removedBall)
    {
        if (overlappingBalls.Contains(removedBall))
        {
            overlappingBalls.Remove(removedBall);
            overlapCount = Mathf.Max(0, overlapCount - 1);
            UpdateColor();
        }
    }

    public bool GetArenaAvailable()
    {
        return overlapCount <= 0;
    }

    public void BeginTimeControl(float lifeTime, int rate)
    {
        Destroy(gameObject, lifeTime); // 5 秒后销毁

        beginTC = true;
        areaRate = rate;
        var worldRadius = GetComponent<SphereCollider>().radius *
                          Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        var hitColliders = Physics.OverlapSphere(transform.position, worldRadius, npcLayer);
        foreach (var hitCollider in hitColliders) npcsInside.Add(hitCollider.GetComponent<IAreaEntityListener>());

        foreach (var entity in npcsInside)
            if (entity != null)
                entity.OnEnterTCArea(direct, rate);
    }

    private void UpdateColor()
    {
        rend.material.SetColor("_Color", overlapCount > 0 ? overlapColor : normalColor);
    }
}