using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Attributes")]
    [SerializeField]
    private int hp = 100;
    [SerializeField]
    private int mana = 50;

    [SerializeField]
    private int ammo = 10;


    [Header("Animator Vriables")]
    private float xInput;
    private float yInput;
    private bool isAlive = true;
    private bool isIdle;
    private bool isAttack;


    [Header("Components")]
    [SerializeField]
    private Animator animator;
    private Transform playerTransform;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerTransform = GetComponent<Transform>();
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
    }
    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        isAttack = Input.GetKey(KeyCode.Mouse0) ? true : false;
        if (isAttack) animator.SetTrigger("isAttack");

        isIdle = xInput == 0 && yInput == 0 ? true : false;

        if (xInput > 0)
        {
            playerTransform.localScale = new Vector3(-1, 1, 1); // Mirror the player
        }
        else if (xInput < 0)
        {
            playerTransform.localScale = new Vector3(1, 1, 1); // Reset to original scale
        }   

        ChangeAnimations();
    }

    private void ChangeAnimations()
    {
        animator.SetFloat("xSpeed", xInput);
        animator.SetFloat("ySpeed", yInput);
        animator.SetBool("isIdle", isIdle);

    }
}
