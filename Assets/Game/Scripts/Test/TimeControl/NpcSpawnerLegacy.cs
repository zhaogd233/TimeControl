using UnityEngine;

public class NpcSpawnerLegacy : MonoBehaviour
{
    [Header("NPC 预制体，应包含 Legacy Animation 和 名为 \"wuqi\" 的特效子对象")]
    public GameObject npcPrefab;


    [Header("生成数量")] public int spawnCount = 10;


    [Header("区域设置")] public Transform areaCenter; // 区域中心

    public float areaHalfSize = 50f; // 半径，50 表示 100×100

    private void Start()
    {
        if (npcPrefab == null)
        {
            Debug.LogError("请在 Inspector 中赋值 npcPrefab");
            return;
        }

        for (var i = 0; i < spawnCount; i++)
        {
            // 随机位置
            var pos = new Vector3(
                Random.Range(areaCenter.position.x - areaHalfSize, areaCenter.position.x + areaHalfSize),
                npcPrefab.transform.position.y,
                Random.Range(areaCenter.position.z - areaHalfSize, areaCenter.position.z + areaHalfSize)
            );
            var rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            var npc = Instantiate(npcPrefab, pos, rot, transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        var size = new Vector3(areaHalfSize * 2, 0.1f, areaHalfSize * 2);
        Gizmos.DrawWireCube(areaCenter.position, size);
    }
}