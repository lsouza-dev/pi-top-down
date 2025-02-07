using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float xInput;
    [SerializeField]
    private float yInput;

    [SerializeField]
    private int hp = 100;
    [SerializeField]
    private int mana = 50;

    [SerializeField]
    private int ammo = 10;

    private bool isAlive = true;
    private bool isIdle;
    private bool isAttack;

    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    private void Awake() {
        animator = GetComponentInChildren<Animator>();    
    }

    void Start()
    {

    }

    private void FixedUpdate() {
    }
    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        isAttack = Input.GetKey(KeyCode.Mouse0) ? true : false; 
        if(isAttack) animator.SetTrigger("isAttack");

        isIdle = xInput == 0 && yInput == 0 ? true : false;
        
        ChangeAnimations();
    }

    private void ChangeAnimations(){
        animator.SetFloat("xSpeed",xInput);
        animator.SetFloat("ySpeed",yInput);
        animator.SetBool("isIdle",isIdle);
        
    }
}
