using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerController playerController;
    private Animator anim;
    [Header("Attributes")]
    [SerializeField] float timeToDestroy = .5f;
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
        var isEnemy = other.gameObject.CompareTag("Enemy");
        var isTower = other.gameObject.CompareTag("TowerEnemy");
        var isSpawner = other.gameObject.CompareTag("Spawner");
        

        if (isEnemy || isTower || isSpawner)
        {
            var dmg = other.gameObject.GetComponent<DamageFeedbackController>();

            if (!isSpawner)
            {
                var enemy = other.gameObject.GetComponent<EnemyController>();
                isCrit = IsCriticalDamage();

                if (isCrit)
                {
                    damage *= 1 + playerController.critDamage / 100f;
                    dmg.color = Color.red;
                }

                enemy.healthBar.timeToDisappear = 5f;
                enemy.healthBar.isActive = true;
                enemy.healthBar.yOffset = .3f;
                enemy.healthBar.UpdateHealthBar();
                enemy.TakeDamage(damage);
            }
            
            if(isSpawner){
                var spawner = other.gameObject.GetComponent<SpawnerController>();
                spawner.healthBar.timeToDisappear = 5f;
                spawner.healthBar.isActive = true;
                spawner.healthBar.yOffset = 1.2f;
                spawner.healthBar.UpdateHealthBar();
                spawner.TakeDamage(damage);
            }

            dmg.ShowDamageFeedback(damage);


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
                timeToDestroy = 3;
                break;
            case "Dagger":
                defaultX = .3f;
                defaultY = .3f;
                break;
            case "Mage":
                timeToDestroy = 3;
                break;

        }

    }
}
