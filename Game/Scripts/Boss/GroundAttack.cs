using UnityEngine;

public class GroundAttack : MonoBehaviour, IBossAttack
{
    public GameObject groundEffectPrefab;

    public void Execute(GameObject player)
    {
        if (player == null || groundEffectPrefab == null) return;

        Vector3 spawnPos = player.transform.position;
        Instantiate(groundEffectPrefab, spawnPos, Quaternion.identity);
    }
}
