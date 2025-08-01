using System;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Color normalColor = Color.white;
    public Color overlapColor = Color.red;

    public LayerMask npcLayer; // 只检测 NPC 层
    private float checkRadius;
    private Renderer rend;
    private int overlapCount = 0;
    private List<Ball> overlappingBalls = new List<Ball>();
    
    public HashSet<IAreaEntityListener> npcsInside = new HashSet<IAreaEntityListener>();
    private bool beginTC;
    void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = normalColor;
        
        // 获取模型的包围盒
        var bounds = GetComponent<MeshRenderer>().bounds;

        // 半径取最大边长的一半（保证覆盖整个球）
        checkRadius = GetSphereColliderRadius();
        Debug.Log(checkRadius);
    }
    float GetSphereColliderRadius()
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        if (sc != null)
        {
            return sc.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        }
        return 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball otherBall = other.GetComponent<Ball>();
            if (otherBall != null && !overlappingBalls.Contains(otherBall))
            {
                overlappingBalls.Add(otherBall);
                overlapCount++;
                UpdateColor();
            }
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            IAreaEntityListener entity = other.gameObject.GetComponent<IAreaEntityListener>();
            if(entity != null)
            npcsInside.Add(entity);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Ball otherBall = other.GetComponent<Ball>();
            if (otherBall != null && overlappingBalls.Contains(otherBall))
            {
                overlappingBalls.Remove(otherBall);
                overlapCount = Mathf.Max(0, overlapCount - 1);
                UpdateColor();
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            IAreaEntityListener entity = other.gameObject.GetComponent<IAreaEntityListener>();
            if (entity != null)
            {
                if (beginTC)
                    entity.OnExitTCArea();
                npcsInside.Remove(entity);
            }
        }
    }
    void OnDestroy()
    {
        // 通知所有重叠的球：“我走了”
        foreach (var otherBall in overlappingBalls)
        {
            if (otherBall != null)
                otherBall.NotifyBallRemoved(this);
        }
        overlappingBalls.Clear();
        
        foreach (IAreaEntityListener entity in npcsInside)
        {
            entity.OnExitTCArea();
        }

        beginTC = false;
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

    public void BeginTimeControl(float lifeTime,float rate)
    {
        Destroy(gameObject, lifeTime); // 5 秒后销毁

        beginTC = true;
        foreach (IAreaEntityListener entity in npcsInside)
        {
            entity.OnEnterTCArea(rate);
        }
    }
  
    void UpdateColor()
    {
        rend.material.SetColor("_Color", (overlapCount > 0) ? overlapColor : normalColor);
    }

    private void Update()
    {
        if (!beginTC)
            return;
        foreach (IAreaEntityListener entity in npcsInside)
        {
            entity.OnStayInTCArea(Time.deltaTime);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}