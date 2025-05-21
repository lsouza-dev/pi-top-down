using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("Timers")]
    //[SerializeField] private float timeToHitGround = 3f;
    [SerializeField] private float timeToDestroy = 8f;
    [SerializeField] private float collisionDamage = 100f;
    [SerializeField] private float onStayAreaDamage = 50f;

    [Header("Components")]
    [SerializeField] private GameObject damageArea;
    [SerializeField] private SpriteRenderer parentSpriteRenderer;
    [SerializeField] private SpriteRenderer areaSpriteRenderer;
    [SerializeField] private CircleCollider2D parentCollider;
    [SerializeField] private CircleCollider2D areaCollider;

    private float elapsedTime = 0f;
    private bool isCoroutineRunning = false;

    private Dictionary<GameObject, EnemyController> cachedEnemies = new();

    void Awake()
    {
        parentSpriteRenderer = GetComponent<SpriteRenderer>();
        parentCollider = GetComponent<CircleCollider2D>();
        areaSpriteRenderer = damageArea.GetComponent<SpriteRenderer>();
        areaCollider = damageArea.GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        areaSpriteRenderer.enabled = false;
        areaCollider.enabled = false;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeToDestroy -= Time.deltaTime;
        PowerUpLifeCycle();
    }

    void PowerUpLifeCycle()
    {
        if (timeToDestroy <= 0)
            Destroy(gameObject);
    }

    public void LaunchMeteorEffect()
    {
        if (!isCoroutineRunning)
            StartCoroutine(WaitToDisableMeteor());
    }

    private IEnumerator WaitToDisableMeteor()
    {
        isCoroutineRunning = true;

        timeToDestroy = 3f;

        parentSpriteRenderer.enabled = false;
        parentCollider.enabled = false;

        areaSpriteRenderer.enabled = true;
        areaCollider.enabled = true;

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("TowerEnemy"))
        {
            if (!cachedEnemies.ContainsKey(other.gameObject))
                cachedEnemies[other.gameObject] = other.GetComponent<EnemyController>();

            var enemy = cachedEnemies[other.gameObject];
            if (enemy != null)
            {
                enemy.healthBar.timeToDisappear = 5f;
                enemy.healthBar.isActive = true;
                enemy.healthBar.yOffset = .3f;
                enemy.healthBar.UpdateHealthBar();
                enemy.TakeDamage(collisionDamage);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("TowerEnemy"))
        {
            if (!cachedEnemies.ContainsKey(other.gameObject))
                cachedEnemies[other.gameObject] = other.GetComponent<EnemyController>();

            var enemy = cachedEnemies[other.gameObject];
            if (enemy != null)
            {
                enemy.healthBar.timeToDisappear = 5f;
                enemy.healthBar.isActive = true;
                enemy.healthBar.yOffset = .3f;
                enemy.healthBar.UpdateHealthBar();
                enemy.TakeDamage(onStayAreaDamage * Time.deltaTime);
            }
        }
    }
}
