using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
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

    private float elapsedTime = 0f;

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
        elapsedTime += Time.deltaTime;
        timeToDestroy -= Time.deltaTime;
        PowerUpLifeCicle();
    }

   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("TowerEnemy"))
        {
            print("Inimigo entrou no PowerUp");
            var enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.healthBar.timeToDisappear = 5f;
            enemy.healthBar.isActive = true;
            enemy.healthBar.yOffset = .3f;
            enemy.healthBar.UpdateHealthBar();
            enemy.TakeDamage(collisionDamage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("TowerEnemy"))
        {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.healthBar.timeToDisappear = 5f;
            enemy.healthBar.isActive = true;
            enemy.healthBar.yOffset = .3f;
            enemy.healthBar.UpdateHealthBar();
            enemy.TakeDamage(onStayAreaDamage);
        }
    }

    private bool isCoroutineRunning = false; // Variável de controle

    private void PowerUpLifeCicle()
    {
        if (SkillController.instance.isOnGround && !isCoroutineRunning)
        {
            StartCoroutine(WaitToDisableMeteor());
        }
        if (timeToDestroy <= 0) Destroy(gameObject);
    }

    private IEnumerator WaitToDisableMeteor()
    {
        isCoroutineRunning = true; // Marca que a coroutine está em execução

        timeToDestroy = 3f;

        parentSpriteRenderer.enabled = false;
        parentCollider.enabled = false;

        areaSpriteRenderer.enabled = true;
        areaCollider.enabled = true;

        yield return new WaitForSeconds(3f);

        Destroy(gameObject); // Destroi o objeto após 3 segundos
        isCoroutineRunning = false; // Reseta a variável de controle (opcional, mas não necessário aqui)
    }
}
