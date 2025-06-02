using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public PlayerController playerController;
    private Animator anim;
    private Rigidbody2D rb;
    [Header("Attributes")]
    [SerializeField] float timeToDestroy = 2f;
    [SerializeField] public float speed = 10f;
    private float bulletDamage = 10f;
    private float followTimer = 0f;
    private bool isFollowing = true;
    private Vector2 direction;
    public MinionSpawner minionSpawner;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        // Inicializa a direção para o player
        if (playerController != null)
        {
            direction = (playerController.transform.position - transform.position).normalized;
        }
    }

    void Update()
    {
        if (isFollowing)
        {
            followTimer += Time.deltaTime;

            if (followTimer >= 2f)
            {
                isFollowing = false;
                minionSpawner.Execute(); // Chama o método de spawn de minions
            }
            else if (playerController != null)
            {
                // Atualiza a direção para seguir o player
                direction = (playerController.transform.position - transform.position).normalized;
            }
        }

        // Move a bullet na direção atual
        rb.velocity = direction * speed;
    }

    void OnEnable()
    {
        // Reseta os valores quando a bullet é reativada
        followTimer = 0f;
        isFollowing = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.TakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }
}
