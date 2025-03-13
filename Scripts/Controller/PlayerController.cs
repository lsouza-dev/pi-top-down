using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] public int level = 1;
    [SerializeField] public float nexLevelPoints = 50;
    [SerializeField] public float currentHp = 100;
    [SerializeField] public float maxHp = 100;
    [SerializeField] public float xp = 00;
    [SerializeField] public float strength = 5;
    [SerializeField] public float atkSpeed = .3f;   

    

    [Header("Animator Variables")]
    private float xInput;
    private float yInput;
    public int mouseDirection;
    private bool isAlive = true;
    private bool isIdle;
    public bool isAttack;

    [Header("Components")]
    [SerializeField] private Animator animator;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private CapsuleCollider2D capCollider;

    [Header("Scripts")]
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private UIAtributtesController uIAtributtesController;

    [Header("Class")]
    [SerializeField] public int evolutionIndex = 0;
    [SerializeField] public int playerClass = 0;
    [SerializeField] public SpriteLibrary library;
    [SerializeField] public SpriteLibraryAsset spriteLibraryAsset;
    [SerializeField] public Bullet currentBullet;
    [SerializeField] public Vector2 colliderOffset ;
    [SerializeField] public Vector2 colliderSize ;
    [SerializeField] public bool isEvolving;


    [Header("Movement")]
    [SerializeField] public float maxSpeed;
    [SerializeField] public float shootCooldownTime = 0;
    [SerializeField] public float shootCooldownTimeDefault = 1.8f;

    public static PlayerController instance;

    [SerializeField] private float stoppedTime;
    [SerializeField] private float invencibleTime;

    [SerializeField] private float distanciamaxdogizmos;
    [SerializeField] private Vector3 mouseLimit;
    [SerializeField] private Vector3[] spawnersOffset;

    private void Awake()
    {
        library = GetComponentInChildren<SpriteLibrary>();
        animator = GetComponentInChildren<Animator>();
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<CapsuleCollider2D>();
        playerAttack = GetComponent<PlayerAttack>();
        playerClass = PlayerPrefs.GetInt("PlayerClass");
        uIAtributtesController = FindObjectOfType<UIAtributtesController>();
    }

    void Start()
    {
        instance = instance == null ? instance = this : instance;
        EvolvePlayer(evolutionIndex, playerClass);
        if (spriteLibraryAsset == null) print($"Sprite Library Asset is Null");
        if (currentBullet == null) print($"Bullet is Null");
        uIAtributtesController.SetAttributesValuesToUI();

    }

    void Update()
    {
        if (!isAlive || spriteLibraryAsset == null || currentBullet == null) return;
        if (stoppedTime <= 0)
        {
            PlayerMovement();
            PlayerAttack();
            UpdateMouseDirection();
            PlayerInvencible();
        }
        if (Input.GetKeyUp(KeyCode.Escape)) SceneManager.LoadScene("Menu");

        uIAtributtesController.SetAttributesValuesToUI();
        ChangeAnimations();
        DecrementTime();
    }

    private void PlayerMovement()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(xInput, yInput);
        float currentSpeed = Mathf.Lerp(0, maxSpeed, direction.magnitude);

        rb.velocity = direction * currentSpeed;

        isIdle = rb.velocity == Vector2.zero;
    }

    private void PlayerAttack()
    {
        isAttack = Input.GetMouseButton(0) && shootCooldownTime <= 0;
        if (!Input.GetMouseButton(0)) animator.SetBool("isAttack", false);

        if (isAttack)
        {
            animator.SetBool("isAttack", true);
            playerAttack.Shoot(currentBullet);
        }
    }

    private void ChangeAnimations()
    {
        animator.SetFloat("xSpeed", xInput);
        animator.SetFloat("ySpeed", yInput);
        animator.SetInteger("mouseDirection", mouseDirection);
        animator.SetBool("isIdle", isIdle);
    }

    private void UpdateMouseDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - playerTransform.position).normalized;

        mouseDirection = GetMouseDirection(direction);
    }

    private int GetMouseDirection(Vector3 direction)
    {
        Vector3 front = playerTransform.up;
        Vector3 right = playerTransform.right;

        float dotFront = Vector3.Dot(direction, front);
        float dotRight = Vector3.Dot(direction, right);

        if (mouseDirection == 1) playerTransform.localScale = new Vector3(-1, 1, 1);
        else playerTransform.localScale = new Vector3(1, 1, 1);

        if (dotFront > 0.7f) return 0;
        if (dotRight > 0) return 1;
        if (dotFront < -0.7f) return 2;
        return 3;
    }

    public void TakeDamage(float damage)
    {
        if (invencibleTime >= 0) return;

        this.currentHp -= damage;
        animator.SetTrigger("isDamage");

        rb.velocity = Vector2.zero;

        if (this.currentHp <= 0)
        {
            isAlive = false;
            animator.SetBool("isAlive", isAlive);
        }

        stoppedTime = .5f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (invencibleTime > 0) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            enemy.PlayerHit(this);
            invencibleTime = 2f;
        }
    }

    private void PlayerInvencible()
    {
        if (invencibleTime > 0)
        {
            float alpha = Mathf.PingPong(Time.time * 5, 1) > 0.5f ? 1f : 0.5f;
            Color color = playerTransform.GetComponentInChildren<SpriteRenderer>().color;
            color.a = alpha;
            playerTransform.GetComponentInChildren<SpriteRenderer>().color = color;
        }
        else
        {
            Color color = playerTransform.GetComponentInChildren<SpriteRenderer>().color;
            color.a = 1f;
            playerTransform.GetComponentInChildren<SpriteRenderer>().color = color;
        }

    }

    private void DecrementTime()
    {
        shootCooldownTime -= Time.deltaTime;
        invencibleTime -= Time.deltaTime;
        stoppedTime -= Time.deltaTime;
    }


    public void EvolvePlayer(int evolution, int classIndex)
    {
        (this.spriteLibraryAsset, this.currentBullet,this.colliderOffset,this.colliderSize,this.spawnersOffset) = ClassSelector.instance.ClassChoice(classIndex);
        this.library.spriteLibraryAsset = spriteLibraryAsset;
        playerAttack.spawnerOffsets = this.spawnersOffset;
        this.capCollider.offset = colliderOffset;
        this.capCollider.size = colliderSize;
    }


    
}