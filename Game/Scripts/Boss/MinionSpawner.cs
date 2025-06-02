using UnityEngine;

public class MinionSpawner : MonoBehaviour, IBossAttack
{
    public GameObject minionPrefab;
    public int numberToSpawn = 3;
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    public void Execute(GameObject player)
    {
        if (minionPrefab == null || player == null) return;

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y),
                0f
            );

            GameObject minion = Instantiate(minionPrefab, pos, Quaternion.identity);
            // O minion usará seu script já existente para atacar o player automaticamente
        }
    }
}
