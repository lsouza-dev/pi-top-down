using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Attributes")]
    [SerializeField] private int hp = 100;
    [SerializeField] private int mana = 50;
    [SerializeField] private int ammo = 10;

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
    private BoxCollider2D bCollider;

    [Header("Scripts")]
    [SerializeField] private PlayerAttack playerAttack; 

    [Header("Movement")]
    [SerializeField] private float maxSpeed;


    [SerializeField]
    public float offsetTime;
    [SerializeField]
    public float offsetTimeDefault;


    public static PlayerController instance;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        bCollider = GetComponent<BoxCollider2D>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        instance = instance == null ? instance = this : instance;
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(xInput, yInput);
        float currentSpeed = Mathf.Lerp(0, maxSpeed, direction.magnitude);
        rb.velocity = direction * currentSpeed;

        isAttack = Input.GetMouseButton(0) && offsetTime <= 0;
        if(!Input.GetMouseButton(0)) animator.SetBool("isAttack", false);
        if(isAttack) {
                animator.SetBool("isAttack", true);
                playerAttack.Shoot();
        }

        offsetTime -= Time.deltaTime;
        isIdle = xInput == 0 && yInput == 0;

        if (mouseDirection == 1) playerTransform.localScale = new Vector3(-1, 1, 1);
        else playerTransform.localScale = new Vector3(1, 1, 1);


        ChangeAnimations();
        UpdateMouseDirection();
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
        Debug.Log(mouseDirection.ToString());
    }

    private int GetMouseDirection(Vector3 direction)
    {
        Vector3 front = playerTransform.up;
        Vector3 right = playerTransform.right;

        float dotFront = Vector3.Dot(direction, front);
        float dotRight = Vector3.Dot(direction, right);

        if (dotFront > 0.7f) return 0;
        if (dotRight > 0) return 1;
        if (dotFront < -0.7f) return 2;
        return 3;
    }
}
