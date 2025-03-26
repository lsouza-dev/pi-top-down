using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Timers")]
    [SerializeField] private float timeToHitGround = 3f;
    [SerializeField] private float timeToDestroy = 8f;
    [SerializeField] private float collisionDamage = 100f;
    [SerializeField] private float onStayAreaDamage = 50f;

    [Header("Components")]
    [SerializeField] private GameObject damageArea;
    [SerializeField] private SpriteRenderer parentSpriteRenderer;
    [SerializeField] private SpriteRenderer areaSpriteRenderer;
    [SerializeField] private CircleCollider2D parentCollider;
    [SerializeField] private CircleCollider2D areaCollider;

    void Awake()
    {
        parentSpriteRenderer = GetComponent<SpriteRenderer>();
        parentCollider = GetComponent<CircleCollider2D>();
        areaSpriteRenderer = damageArea.GetComponent<SpriteRenderer>();
        areaCollider = damageArea.GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        areaSpriteRenderer.enabled = false;
        areaCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;

        PowerUpLifeCicle();
    }

    private void PowerUpLifeCicle()
    {
        if (PowerUpController.instance.isOnGround)
        {
            StartCoroutine(WaitToDisableMeteor());
        }
        if(timeToDestroy <= 0) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("TowerEnemy"))
        {
            print("Inimigo entrou no PowerUp");
            var enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.TakeDamage(collisionDamage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("TowerEnemy"))
        {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.TakeDamage(onStayAreaDamage);
        }
    }

    private IEnumerator WaitToDisableMeteor()
    {
        yield return new WaitForSeconds(.5f);
        parentSpriteRenderer.enabled = false;
        parentCollider.enabled = false;

        areaSpriteRenderer.enabled = true;
        areaCollider.enabled = true;
        timeToDestroy = 3f;
    }
}
