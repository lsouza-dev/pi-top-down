using System.Collections;
using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged
}

public class MinionController : Enemy
{
    [Header("Comportamento")]
    public AttackType attackType;
    public float attackDistance = 1.5f;
    public float moveSpeed = 3.5f;
    public float attackCooldown = 2f;
    private float currentAttackCooldown;

    [Header("Spawn")]
    public float startDelay = 1f;
    private bool canAct = false;

    [Header("Referências")]
    public CircleCollider2D attackCollider;
    private PlayerController playerController;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public EnemyHealthBar healthBar;

    private float distanceToPlayer;
    private bool isBack;
    private bool isWalking;

    void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (healthBar != null)
            healthBar.minionController = this;

        StartCoroutine(EnableActionsAfterDelay(startDelay));
    }

    IEnumerator EnableActionsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canAct = true;
    }

    void Update()
    {
        if (!isAlive || !canAct || player == null) return;

        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        currentAttackCooldown -= Time.deltaTime;

        UpdateFacingDirection();
        HandleBehavior();
        UpdateAnimations();
    }

    void HandleBehavior()
    {
        if (distanceToPlayer <= attackDistance)
        {
            StopMovement();

            if (currentAttackCooldown <= 0f)
            {
                Attack();
                currentAttackCooldown = attackCooldown;
            }
        }
        else
        {
            ChasePlayer(); // Segue o player o tempo todo
        }
    }

    void StopMovement()
    {
        isWalking = false;
        rb.velocity = Vector2.zero;
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        isWalking = true;
    }

    void Attack()
    {
        animator.SetTrigger("isAttack");

        if (attackType == AttackType.Melee && attackCollider != null)
        {
            attackCollider.enabled = true;
            StartCoroutine(DisableMeleeCollider(0.5f));
        }
        else if (attackType == AttackType.Ranged && projectilePrefab != null && projectileSpawnPoint != null)
        {
            Vector3 dir = (player.transform.position - projectileSpawnPoint.position).normalized;
            GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
            projRb.velocity = dir * 6f; // ajuste a velocidade conforme necessário
        }
    }

    IEnumerator DisableMeleeCollider(float time)
    {
        yield return new WaitForSeconds(time);
        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    void UpdateFacingDirection()
    {
        isBack = transform.position.y < player.transform.position.y;

        if (player.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isBack", isBack);
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
        isAlive = false;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("isDie");
        animator.SetBool("isAlive", false);

        if (healthBar != null)
            Destroy(healthBar.gameObject);

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
            collider.enabled = false;

        Destroy(gameObject, 2f);
        GiveXP();
    }

    void GiveXP()
    {
        if (player == null) return;

        playerController.xp += 20;

        LevelUpController levelUp = FindObjectOfType<LevelUpController>();
        if (playerController.xp >= playerController.nexLevelPoints)
        {
            levelUp.LevelUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.TakeDamage(5f);
        }
    }
}
