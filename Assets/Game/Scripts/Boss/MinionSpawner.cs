using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public RootController rootController; // <- prefab de minion
    public int numberToSpawn = 3;
    private GameObject bossArea;
    private BoxCollider2D bossAreaCollider;

    void Awake()
    {
        bossArea = GameObject.FindGameObjectWithTag("BossArea");
        if (bossArea != null)
        {
            bossAreaCollider = bossArea.GetComponent<BoxCollider2D>();
        }
    }

    public void Execute()
    {
        if (bossAreaCollider == null) return;

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector2 randomPoint = GetRandomPointInBossArea();
            Vector3 spawnPosition = new Vector3(randomPoint.x, randomPoint.y, 0f);

            Instantiate(rootController, spawnPosition, Quaternion.identity);
        }
    }

    private Vector2 GetRandomPointInBossArea()
    {
        Bounds bounds = bossAreaCollider.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}
