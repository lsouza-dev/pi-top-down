using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerController playerController;
    private Animator anim;
    [Header("Attributes")]
    [SerializeField] float timeToDestroy = 2f;
    [SerializeField] public float damage;
    [SerializeField] bool isCrit = false;
    [SerializeField] float defaultX = 1f;
    [SerializeField] float defaultY = 1f;
    [SerializeField] float incrementValue = .01f;
    [SerializeField] float scaleX;
    [SerializeField] float scaleY;
    [SerializeField] public float speed = 10f;

    [Header("Compontents")]
    [SerializeField] SpriteRenderer sr;

    [SerializeField] public string bulletName;

    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        sr = GetComponentInChildren<SpriteRenderer>();
        damage = playerController.strength;
        anim = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletName = this.name.Replace("(Clone)", "");
        BulletClass(bulletName);

        Destroy(gameObject, timeToDestroy);
        transform.localScale = new Vector2(defaultX, defaultY);
        scaleX = defaultX;
        scaleY = defaultY;
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletName == "Dagger")
        {
            transform.localScale = new Vector2(scaleX, scaleY);
            scaleX += incrementValue;
            scaleY += incrementValue;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var isEnemy = other.gameObject.CompareTag("Enemy") && !other.gameObject.name.Contains("Minion");
        var isMinion = other.gameObject.CompareTag("Enemy") && other.gameObject.name.Contains("Minion");
        var isTower = other.gameObject.CompareTag("TowerEnemy");
        var isSpawner = other.gameObject.CompareTag("Spawner");

        if (isEnemy || isTower || isSpawner || isMinion)
        {
            var dmg = other.gameObject.GetComponent<DamageFeedbackController>();

            if (!isSpawner)
            {
                isCrit = IsCriticalDamage();

                if (isCrit)
                {
                    damage *= 1 + playerController.critDamage / 100f;
                    dmg.color = Color.red;
                }

                if (isMinion)
                {
                    var minion = other.gameObject.GetComponent<MinionController>();
                    minion.healthBar.timeToDisappear = 5f;
                    minion.healthBar.isActive = true;
                    minion.healthBar.yOffset = .3f;
                    minion.healthBar.UpdateHealthBar();
                    minion.TakeDamage(damage);
                }
                else
                {
                    var enemy = other.gameObject.GetComponent<EnemyController>();
                    enemy.healthBar.timeToDisappear = 5f;
                    enemy.healthBar.isActive = true;
                    enemy.healthBar.yOffset = .3f;
                    enemy.healthBar.UpdateHealthBar();
                    enemy.TakeDamage(damage);
                }
            }

            if (isSpawner)
            {
                var spawner = other.gameObject.GetComponent<SpawnerController>();
                spawner.healthBar.timeToDisappear = 5f;
                spawner.healthBar.isActive = true;
                spawner.healthBar.yOffset = 1.2f;
                spawner.healthBar.UpdateHealthBar();
                spawner.TakeDamage(damage);
            }

            dmg.ShowDamageFeedback(damage, isCrit);


            if (bulletName == "Dagger")
            {
                damage *= .90f;
                Color color = sr.color;
                if (color.a <= 0) Destroy(gameObject);

                color.a = Mathf.Clamp01(color.a - .15f);
                sr.color = color;
            }
            if (bulletName == "Mage" || bulletName == "Archer")
            {
                anim.SetTrigger("explode");
                var rb = GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                Destroy(gameObject, .5f);
            }
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Objects")) Destroy(gameObject);

    }

    private bool IsCriticalDamage()
    {

        var critRate = playerController.critRate / 100f;
        var critChance = Random.Range(0f, 1f);

        if (critChance <= critRate) return true;
        return false;
    }


    private void BulletClass(string bulletName)
    {
        switch (bulletName)
        {
            case "Archer":
                timeToDestroy = 5;
                defaultX *= 2f;
                defaultY *= 2f;
                break;
            case "Dagger":
                defaultX = .3f;
                defaultY = .3f;
                break;
            case "Mage":
                timeToDestroy = 5;
                defaultX *= 2.5f;
                defaultY *= 2.5f;
                break;

        }

    }
}
