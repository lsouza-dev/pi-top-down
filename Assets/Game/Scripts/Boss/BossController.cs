using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class BossController : MonoBehaviour
{
    [Header("Movimentação")]
    public Vector2 areaMin;
    public Vector2 areaMax;
    public float moveSpeed = 10f;
    public float idleDuration = 2f;
    private float idleTimer = 0f;
    private Vector3 lastPosition;
    private bool movingRight = false;
    private float changeDirectionChance = 0.01f; // 1% de chance de mudar de direção a cada frame

    [Header("Ataques")]
    public float attackInterval = 5f;
    public float spawnInterval = 15f;
    private float nextAttackTime;
    private float nextSpawnTime;

    [Header("Referências")]
    public GameObject player;
    public MinionSpawner minionSpawner;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] public EnemyHealthBar healthBar;
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("Animações")]
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isWalking;
    [SerializeField] public bool isAlive = true;

    [SerializeField] private List<BossBullet> bullets = new List<BossBullet>();
    private BossBullet currentBullet;
    private LevelUpController levelUpController;
    [SerializeField] public List<MinionController> minions = new List<MinionController>();
    
    private MinionController currentMinion;
    private RootController rootController;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        minionSpawner = GetComponent<MinionSpawner>();
        lastPosition = transform.position;
        levelUpController = FindObjectOfType<LevelUpController>();
        rootController = Resources.Load<RootController>("Root/Root");
    }

    void Start()
    {
        bullets = Resources.LoadAll<BossBullet>("Bullets\\Boss").ToList();
        minions = Resources.LoadAll<MinionController>("Minions").ToList();
        currentBullet = bullets.FirstOrDefault();
        currentMinion = minions.FirstOrDefault();
        currentBullet.minionSpawner = minionSpawner;
        minionSpawner.rootController = rootController;
        
        if (isAlive)
        {
            nextAttackTime = Time.time + attackInterval;
            nextSpawnTime = Time.time + spawnInterval;
            StartCoroutine(BossActionRoutine());
        }
    }

    void Update()
    {
        if (isAlive)
        {
            HandleMovement();
            UpdateAnimations();
        }
    }

    void HandleMovement()
    {
        if (idleTimer >= idleDuration) MoveToRandomPosition();
        else idleTimer += Time.deltaTime;
    }

    void MoveToRandomPosition()
    {
        float moveStep = moveSpeed * Time.deltaTime;
        Vector3 newPosition;

        // Inverter direção com baixa chance aleatória (sem estar nos limites)
        if (Random.value < changeDirectionChance &&
            transform.position.x > areaMin.x + 0.5f &&
            transform.position.x < areaMax.x - 0.5f)
        {
            movingRight = !movingRight;
        }

        // Movimento normal com verificação de limites
        if (movingRight)
        {
            newPosition = new Vector3(transform.position.x + moveStep, transform.position.y, transform.position.z);

            if (newPosition.x >= areaMax.x)
            {
                newPosition.x = areaMax.x;
                movingRight = false;
            }
        }
        else
        {
            newPosition = new Vector3(transform.position.x - moveStep, transform.position.y, transform.position.z);

            if (newPosition.x <= areaMin.x)
            {
                newPosition.x = areaMin.x;
                movingRight = true;
            }
        }

        rb.MovePosition(Vector3.MoveTowards(transform.position, newPosition, moveStep));
        isWalking = true;
    }

    IEnumerator BossActionRoutine()
    {
        while (isAlive)
        {
            float currentTime = Time.time;

            // Se ambos os tempos chegarem juntos (com margem de 0.1 segundos)
            if (Mathf.Abs(nextAttackTime - nextSpawnTime) < 0.1f)
            {
                // Prioriza o ataque
                print("Boss is attacking...");
                ExecuteAttack();
                nextAttackTime = currentTime + attackInterval;
                nextSpawnTime = currentTime + spawnInterval;
            }
            else
            {
                // Verifica qual ação deve ser executada primeiro
                if (nextAttackTime <= currentTime)
                {
                    print("Boss is attacking...");
                    ExecuteAttack();
                    nextAttackTime = currentTime + attackInterval;
                }

                if (nextSpawnTime <= currentTime)
                {
                    print("Boss is spawning minions...");   
                    minionSpawner.Execute(); // Executa o spawn de minions
                    nextSpawnTime = currentTime + spawnInterval;
                }
            }

            yield return new WaitForSeconds(0.1f); // Pequeno intervalo para não sobrecarregar
        }
    }

    public void TakeDamage(float damage)
    {
        this.currentHealth -= damage;
        animator.SetTrigger("isDamage");
        if (this.currentHealth <= 0)
        {
            GameManager.Instance.SetBossKilled();
            print("o boss morreu, a porta ta aberta");
            isAlive = false;
            var player = FindObjectOfType<PlayerController>();
            player.xp += 20;
            rb.velocity = Vector2.zero;

            if (player.xp >= player.nexLevelPoints) levelUpController.LevelUp();

            animator.SetTrigger("isDie");
            animator.SetBool("isAlive", false);

            CapsuleCollider2D coll = GetComponent<CapsuleCollider2D>();
            coll.enabled = false;
        }
    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isAlive", isAlive);

        // O parâmetro "hit" pode ser setado de forma externa ao levar dano
        // animator.SetTrigger("hit");
    }

    void ExecuteAttack()
    {
        if (bullets.Count > 0 && bulletSpawnPoint != null)
        {
            animator.SetTrigger("isAttack");
            BossBullet bullet = Instantiate(currentBullet, bulletSpawnPoint.transform.position, Quaternion.identity);
            bullet.playerController = player.GetComponent<PlayerController>();
        }
    }
}
