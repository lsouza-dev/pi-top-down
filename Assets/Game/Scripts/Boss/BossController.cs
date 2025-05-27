using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossController : MonoBehaviour
{
    [Header("Movimentação")]
    public Vector2 areaMin;
    public Vector2 areaMax;
    public float moveSpeed = 10f;
    public float idleDuration = 2f;
    private Vector3 targetPosition;
    private float idleTimer = 0f;
    private Vector3 lastPosition;
    private bool movingRight = false;
    private float changeDirectionChance = 0.01f; // 1% de chance de mudar de direção a cada frame

    [Header("Ataques")]
    public float attackInterval = 3f;
    public float minionSpawnInterval = 6f;
    private float attackTimer;
    private float minionSpawnTimer;
    private int currentAttackIndex = 0;

    [Header("Referências")]
    public GameObject player;
    public List<MonoBehaviour> attackBehaviors;
    public MinionSpawner minionSpawner;

    [Header("Animações")]
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isAlive = true;

    private List<IBossAttack> attacks = new List<IBossAttack>();

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        lastPosition = transform.position;
        foreach (var behavior in attackBehaviors)
        {
            if (behavior is IBossAttack attack)
                attacks.Add(attack);
        }
    }

    void Update()
    {
        if (isAlive)
        {
            HandleMovement();
            HandleAttacks();
            HandleMinionSpawn();
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

    void HandleAttacks()
    {
        if (attackTimer <= 0f)
        {
            attackTimer = attackInterval;
            if (attacks.Count > 0)
            {
                attacks[currentAttackIndex].Execute(player);
                currentAttackIndex = (currentAttackIndex + 1) % attacks.Count;
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void HandleMinionSpawn()
    {
        if (minionSpawnTimer <= 0f)
        {
            minionSpawnTimer = minionSpawnInterval;
            if (minionSpawner != null)
                minionSpawner.Execute(player);
        }
        else
        {
            minionSpawnTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        // Lógica para receber dano
        // Exemplo: Reduzir vida, verificar se está morto, etc.
        isAlive = true; // Defina como false se o boss morrer
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isAlive", isAlive);

        // O parâmetro "hit" pode ser setado de forma externa ao levar dano
        // animator.SetTrigger("hit");
    }
}
