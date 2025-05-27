using UnityEngine;

public class GroundAttack : MonoBehaviour, IBossAttack
{
    public GameObject groundEffectPrefab;

    public void Execute(GameObject player)
    {
        if (player == null || groundEffectPrefab == null) return;

        Vector3 spawnPos = transform.position + Vector3.down * 1f;
        Instantiate(groundEffectPrefab, spawnPos, Quaternion.identity);
    }
}
