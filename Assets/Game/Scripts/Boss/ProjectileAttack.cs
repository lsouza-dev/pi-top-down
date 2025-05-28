using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public Transform firePoint;

    public void Execute(GameObject player)
    {
        if (player == null || projectilePrefab == null || firePoint == null) return;

        Vector3 direction = (player.transform.position - firePoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }
}
