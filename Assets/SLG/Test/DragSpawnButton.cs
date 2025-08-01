using UnityEngine;
using UnityEngine.EventSystems;

public class DragSpawnButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject ballPrefab;
    
    public Camera worldCamera; // 用来投射到世界的相机

    private GameObject currentBall;
    public float ballLifeTime = 5;
    public float rate = 1f;
    public void OnBeginDrag(PointerEventData eventData)
    {
        SpawnBall(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentBall != null)
        {
            MoveBall(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ball ball =  currentBall.GetComponent<Ball>();
        if(ball != null && ball.GetArenaAvailable())
            ball.BeginTimeControl(ballLifeTime,rate);
        else
        {
            Destroy(currentBall);
        }
        currentBall = null; // 放手，不再控制
    }

    private void SpawnBall(PointerEventData eventData)
    {
        Vector3 spawnPos = GetWorldPosition(eventData);
        currentBall = Instantiate(ballPrefab, spawnPos, ballPrefab.transform.rotation);
        currentBall.SetActive(true);
    }

    private void MoveBall(PointerEventData eventData)
    {
        Vector3 spawnPos = GetWorldPosition(eventData);
        currentBall.transform.position = new Vector3(spawnPos.x, -4, spawnPos.z);
    }

    private Vector3 GetWorldPosition(PointerEventData eventData)
    {
        Ray ray = worldCamera.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}