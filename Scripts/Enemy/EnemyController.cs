using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Enemy
{
    [SerializeField] float chaseDistance, atkDistance;
    [SerializeField] float patrolArea;
    private Vector2 rndPatrolPos;
    private float patrolTimer;
    private float distance;

    [SerializeField] float collisionDamage = 5f;

    private float spd = 1;
    [SerializeField] private BoxCollider2D frontMeleeCollider;
    [SerializeField] private BoxCollider2D backMeleeCollider;

    private LevelUpController levelUpController;
    [SerializeField] private float attackDelay;


    [Header("Animator Variables")]
    private bool isWalking;
    private bool isBack;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponentInChildren<Animator>();
        levelUpController = FindObjectOfType<LevelUpController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (this.gameObject.tag != "TowerEnemy")
        {
            frontMeleeCollider.enabled = false;
            backMeleeCollider.enabled = false;
        }
    }

    public void Update()
    {
        if (!isAlive) return;
        distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance < atkDistance)
        {
            if (this.gameObject.tag == "TowerEnemy")
            {
                print("TowerEnemy");
            }
            else
            {
                if (isBack) this.backMeleeCollider.enabled = true;
                else this.frontMeleeCollider.enabled = true;
            }

            AttackPlayer();
        }
        else if (distance < chaseDistance && this.gameObject.tag != "TowerEnemy")
        {
            this.frontMeleeCollider.enabled = false;
            this.backMeleeCollider.enabled = false;
            ChasePlayer();
        }
        else
        {
            if (this.gameObject.tag != "TowerEnemy")
            {

                this.frontMeleeCollider.enabled = false;
                this.backMeleeCollider.enabled = false;
                Patrol();
            }
        }

        ChangeAnimations();

        UpdatePlayerTransform();

        if (transform.position.y < player.transform.position.y) isBack = true;
        else isBack = false;

        attackDelay -= Time.deltaTime;
    }

    void Patrol()
    {
        isWalking = true;

        if (patrolTimer <= 0)
        {
            rndPatrolPos = new Vector2(transform.position.x + Random.Range(-patrolArea, patrolArea),
                                       transform.position.y + Random.Range(-patrolArea, patrolArea));

            patrolTimer = Random.Range(5f, 7f);
        }

        transform.position = Vector2.MoveTowards(transform.position, rndPatrolPos, spd * Time.deltaTime);

        if (patrolTimer > 0)
        {
            patrolTimer -= Time.deltaTime;
        }

        if (rndPatrolPos.x > transform.position.x)
        {
            transform.localScale = new(-1, 1);
        }

        if (rndPatrolPos.x < transform.position.x)
        {
            transform.localScale = new(1, 1);
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, spd * Time.deltaTime);
        isWalking = true;
    }

    void AttackPlayer()
    {
        if (attackDelay <= 0)
        {
            animator.SetTrigger("isAttack");
            attackDelay = 2f;
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }
    }

    public void TakeDamage(float damage)
    {
        this.health -= damage;
        animator.SetTrigger("isDamage");
        if (this.health <= 0)
        {
            var player = FindObjectOfType<PlayerController>();
            player.xp += 20;
            rb.velocity = Vector2.zero;
            isAlive = false;

            BoxCollider2D parentCollider = GetComponentInParent<BoxCollider2D>();
            parentCollider.enabled = false;

            if(this.gameObject.tag != "TowerEnemy") Destroy(gameObject, 2f);
            else isAlive = false;

            if (player.xp >= player.nexLevelPoints) levelUpController.LevelUp();

            animator.SetTrigger("isDie");
            animator.SetBool("isAlive", isAlive);
        }
    }

    public void UpdatePlayerTransform()
    {
        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new(1, 1);
        }

        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new(-1, 1);
        }
    }

    public void PlayerHit(PlayerController player)
    {
        print("Player Hit");
        player.TakeDamage(collisionDamage);
    }

    private void ChangeAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isBack", isBack);
    }
}