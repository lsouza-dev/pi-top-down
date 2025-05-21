using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Movimentação")]
    public Vector2 areaMin;
    public Vector2 areaMax;
    public float moveSpeed = 2f;
    private Vector3 targetPosition;

    [Header("Ataques")]
    public float[] attackIntervals = { 3f, 6f, 9f };
    private float attackTimer;
    private int currentAttackIndex = 0;

    [Header("Referências")]
    public GameObject player;
    public List<MonoBehaviour> attackBehaviors;

    private List<IBossAttack> attacks = new List<IBossAttack>();

    private void Start()
    {
        foreach (var behavior in attackBehaviors)
        {
            if (behavior is IBossAttack attack)
                attacks.Add(attack);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        SetNewTargetPosition();
    }

    private void Update()
    {
        MoveWithinArea();

        if (player == null) return;

        attackTimer += Time.deltaTime;
        if (currentAttackIndex < attackIntervals.Length && attackTimer >= attackIntervals[currentAttackIndex])
        {
            ExecuteRandomAttack();
            currentAttackIndex++;
        }
    }

    void MoveWithinArea()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        float x = Random.Range(areaMin.x, areaMax.x);
        float y = Random.Range(areaMin.y, areaMax.y);
        targetPosition = new Vector3(x, y, transform.position.z);
    }

    void ExecuteRandomAttack()
    {
        if (attacks.Count == 0) return;

        int index = Random.Range(0, attacks.Count);
        attacks[index].Execute(player);
    }
}
