using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public List<EnemyController> enemies = new List<EnemyController>();
    public List<EnemyController> spawnedEnemies = new List<EnemyController>();
    
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CapsuleCollider2D capsuleCollider;
    [SerializeField] private Animator animator;

    [Header("Spawner Variables")]
    private float health = 30f;
    private bool isIdle;
    private bool isRespawning;
    
    public static SpawnerController instance;
    public int maxEnemiesOnSpawner;
    public string sceneName;

    void Awake()
    {
        instance = instance == null ? this : instance;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }


    void Start()
    {
        capsuleCollider.enabled = false;
        sceneName = GameController.instance.sceneName;
        if (sceneName != null)
        {
            GetMaxEnemiesSpawn(sceneName);
            SpawnEnemies();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var enemiesAlive = spawnedEnemies.Where(e => e != null).ToList();

        if (enemiesAlive.Count == 0)
        {
            capsuleCollider.enabled = true;
            isRespawning = true;
            isIdle = false;
        }else{
            capsuleCollider.enabled = false;
            isRespawning = false;
            isIdle = true;
        }

        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isRespawning", isRespawning);
        animator.SetFloat("health", health);
    }


    private void GetMaxEnemiesSpawn(string sceneName)
    {
        switch (sceneName)
        {
            case "Forest":
                enemies = Resources.LoadAll<EnemyController>("Enemy\\Forest").ToList();
                break;
            case "Desert":
                enemies = Resources.LoadAll<EnemyController>("Enemy\\Desert").ToList();
                break;
            case "Snow":
                enemies = Resources.LoadAll<EnemyController>("Enemy\\Snow").ToList();
                break;
            case "Swamp":
                enemies = Resources.LoadAll<EnemyController>("Enemy\\Swamp").ToList();
                break;
            default:
                enemies = null;
                break;
        }

        maxEnemiesOnSpawner = PlayerPrefs.GetInt("maxEnemiesOnSpawner");
    }

    private void SpawnEnemies()
    {
        for (int i = 1; i <= maxEnemiesOnSpawner; i++)
        {
            int randomEnemy = Random.Range(0, enemies.Count);
            var enemy = enemies[randomEnemy];
            enemy.spawner = gameObject;

            if (spawnedEnemies.Count < maxEnemiesOnSpawner)
            {
                var instantiatedEnemy = Instantiate(enemy, new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1), 0), Quaternion.identity);
                spawnedEnemies.Add(instantiatedEnemy);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Bullet")){
            var damage = other.gameObject.GetComponentInParent<Bullet>().damage;
            TakeDamage(damage);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        print("Spawner took damage: " + damage);
        print("Spawner health: " + health);
    }

    private IEnumerator RespawnEnemies()
    {
        SpawnEnemies();
        yield return new WaitForSeconds(5f);
    }
}
