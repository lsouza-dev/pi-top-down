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

    private float spd = 1;
    [SerializeField] private BoxCollider2D meleeCollider;

    void Awake()
    {
        meleeCollider = GameObject.FindGameObjectWithTag("MeleeCollider").GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {

        distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance < atkDistance)
        {
            print("ataque");
            meleeCollider.enabled = true;
        }
        else if (distance < chaseDistance)
        {
            ChasePlayer();
            meleeCollider.enabled = false;
        }
        else
        {
            Patrol();
            meleeCollider.enabled = false;
        }
    }

    void Patrol()
    {
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

        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new(-1, 1);
        }

        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new(1, 1);
        }
    }

    // void ChargeAtk()
    // {
    //     anim.SetTrigger("Charge");
    // }
}
