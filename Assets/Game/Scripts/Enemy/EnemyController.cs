using System.Collections;
using UnityEngine;

public class EnemyController : Enemy
{
    [SerializeField] float chaseDistance = 5f, atkDistance = 1f;
    [SerializeField] float patrolArea = 3f;
    [SerializeField] float collisionDamage = 5f;
    [SerializeField] float attackDelay = 2f;
    [SerializeField] BoxCollider2D frontMeleeCollider, backMeleeCollider;
    [SerializeField] public GameObject spawner;
    [SerializeField] LevelUpController levelUpController;
    [SerializeField] public EnemyHealthBar healthBar;

    float spd = 1f;
    float patrolTimer;
    float attackCooldown;
    Vector2 rndPatrolPos;
    bool changeDirection;
    bool isWalking, isBack;

    public static EnemyController instance;

    void Awake()
    {
        currentHealth = maxHealth;
        instance = instance == null ? this : instance;

        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        levelUpController = FindObjectOfType<LevelUpController>();

        if (healthBar.enemyController != null)
            healthBar.enemyController = this;
    }

    void Start()
    {
        if (gameObject.tag != "TowerEnemy")
        {
            frontMeleeCollider.enabled = false;
            backMeleeCollider.enabled = false;
        }
    }

    void Update()
    {
        if (!isAlive) return;

        float distance = Vector2.Distance(player.transform.position, transform.position);
        float spawnerDistance = Vector2.Distance(spawner.transform.position, transform.position);

        if (!changeDirection)
            HandleBehavior(distance);

        if (spawnerDistance > 6f)
            rndPatrolPos = spawner.transform.position;

        UpdateAnimations();
        UpdateFacingDirection();
        attackCooldown -= Time.deltaTime;
    }

    void HandleBehavior(float distance)
    {
        if (distance < atkDistance)
        {
            if (gameObject.tag != "TowerEnemy")
            {
                if (transform.position.y < player.transform.position.y)
                    backMeleeCollider.enabled = true;
                else
                    frontMeleeCollider.enabled = true;

                AttackPlayer();
            }
        }
        else if (distance < chaseDistance && gameObject.tag != "TowerEnemy")
        {
            changeDirection = false;
            frontMeleeCollider.enabled = false;
            backMeleeCollider.enabled = false;
            ChasePlayer();
        }
        else if (gameObject.tag != "TowerEnemy")
        {
            frontMeleeCollider.enabled = false;
            backMeleeCollider.enabled = false;
            Patrol();
        }
    }

    void Patrol()
    {
        isWalking = true;

        if (patrolTimer <= 0)
        {
            rndPatrolPos = new Vector2(
                transform.position.x + Random.Range(-patrolArea, patrolArea),
                transform.position.y + Random.Range(-patrolArea, patrolArea)
            );
            patrolTimer = Random.Range(2f, 5f);
        }

        transform.position = Vector2.MoveTowards(transform.position, rndPatrolPos, spd * Time.deltaTime);
        patrolTimer -= Time.deltaTime;

        UpdateScale(rndPatrolPos.x);
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, spd * Time.deltaTime);
        isWalking = true;
        UpdateScale(player.transform.position.x);
    }

    void AttackPlayer()
    {
        if (attackCooldown <= 0)
        {
            animator.SetTrigger("isAttack");
            attackCooldown = attackDelay;
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Objects"))
        {
            Vector2 pushBackDir = (transform.position - collision.transform.position).normalized;
            rndPatrolPos = (Vector2)transform.position + pushBackDir * patrolArea;

            changeDirection = true;
            StartCoroutine(AtivarMovimentacao());
        }
    }

    IEnumerator AtivarMovimentacao()
    {
        Patrol();
        yield return new WaitForSeconds(1f);
        changeDirection = false;
        patrolTimer = 0;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("isDamage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        var player = FindObjectOfType<PlayerController>();
        player.xp += 2;
        rb.velocity = Vector2.zero;
        isAlive = false;

        GetComponent<BoxCollider2D>().enabled = false;
        if (gameObject.tag != "TowerEnemy")
            Destroy(gameObject, 2f);

        if (player.xp >= player.nexLevelPoints)
            levelUpController.LevelUp();

        animator.SetTrigger("isDie");
        animator.SetBool("isAlive", false);
    }

    void UpdateFacingDirection()
    {
        isBack = transform.position.y < player.transform.position.y;
    }

    void UpdateScale(float targetX)
    {
        transform.localScale = new Vector3(targetX > transform.position.x ? 1 : -1, 1, 1);
    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isBack", isBack);
    }

    public void PlayerHit(PlayerController player)
    {
        player.TakeDamage(collisionDamage);
    }
}
