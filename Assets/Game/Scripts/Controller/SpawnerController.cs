using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public List<EnemyController> enemies = new List<EnemyController>();
    public List<EnemyController> spawnedEnemies = new List<EnemyController>();
    public EnemyHealthBar healthBar;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Animator animator;

    [Header("Spawner Variables")]
    public float currentHealth;
    public float maxHealth = 30f;
    private bool isIdle;
    private bool isRespawning;
    [SerializeField] private bool isRespawningCoroutineRunning = false; // Controle da corrotina

    public static SpawnerController instance;
    [SerializeField] private int enemiesSpawnedsCount;
    [SerializeField] private int maxEnemiesSpawneds = 5;
    public string sceneName;

    private static int totalSpawners = 0;
    private static int destroySpawners = 0;

    void Awake()
    {
        instance = instance == null ? this : instance;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        if (healthBar.spawnerController != null) healthBar.spawnerController = this;

        totalSpawners = FindObjectsOfType<SpawnerController>().Count();
        print("spawner criado. Total atual:" + totalSpawners);
    }

    void Start()
    {
        boxCollider.enabled = false;
        sceneName = GameController.instance.sceneName;
        if (sceneName != null)
        {
            GetMaxEnemiesSpawn(sceneName);
            SpawnEnemies();
        }
    }

    void Update()
    {
        var enemiesAlive = spawnedEnemies.Where(e => e != null).ToList();

        if (enemiesAlive.Count == 0)
        {
            boxCollider.enabled = true;
            isRespawning = true;
            isIdle = false;

            if (!isRespawningCoroutineRunning) 
            {
                StartCoroutine(RespawnEnemies());
                print("Corrotina RespawnEnemies iniciada.");
            }
        }
        else
        {
            boxCollider.enabled = false;
            isRespawning = false;
            isIdle = true;
        }

        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isRespawning", isRespawning);
        animator.SetFloat("health", currentHealth);
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
                this.sceneName = "Desert";
                break;
        }
    }

    private void SpawnEnemies()
    {
        spawnedEnemies.Clear();
        enemiesSpawnedsCount++;

        for (int i = 1; i <= enemiesSpawnedsCount; i++)
        {
            int randomEnemy = Random.Range(0, enemies.Count);
            var enemy = enemies[randomEnemy];
            enemy.spawner = gameObject;

            if (enemiesSpawnedsCount < maxEnemiesSpawneds)
            {
                var instantiatedEnemy = Instantiate(enemy, new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1), 0), Quaternion.identity);
                spawnedEnemies.Add(instantiatedEnemy);
            }
        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Bullet"))
    //     {
    //         var damage = other.gameObject.GetComponent<Bullet>().damage;

    //         Destroy(other.gameObject);

    //         var dmgController = GetComponent<DamageFeedbackController>();
    //         dmgController.ShowDamageFeedback(damage);
            
    //         TakeDamage(damage);

    //         Debug.Log("Ai papai, tomei dano: " + damage);
    //     }
    // }

    public void TakeDamage(float damage)
    {
        print($"Health: {currentHealth} - Damage: {damage} - Life Remain: {currentHealth - damage}");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        destroySpawners++;
        print($"Spawners Destruidos: {destroySpawners} - Total atual:{totalSpawners - destroySpawners}");
        if (destroySpawners >= totalSpawners)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetAllSpawnersDestroyed();
                print("Labirinto Destrancado");
            }
            else
                print("Instancia null");
        }
        Destroy(gameObject);
    }

    private IEnumerator RespawnEnemies()
    {
        isRespawningCoroutineRunning = true;
        yield return new WaitForSeconds(5f);
        SpawnEnemies();
        isRespawningCoroutineRunning = false;
    }   
}
