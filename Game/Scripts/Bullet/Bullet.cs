using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerController playerController;

    [Header("Attributes")]
    [SerializeField] float timeToDestroy = .5f;
    [SerializeField] public float damage;
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
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("TowerEnemy"))
        {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.healthBar.isActive = true;
            enemy.healthBar.UpdateHealthBar();
            enemy.TakeDamage(damage);

            if (bulletName == "Dagger")
            {
                damage *= .90f;
                Color color = sr.color;
                if (color.a <= 0) Destroy(gameObject);

                color.a = Mathf.Clamp01(color.a - .15f);
                sr.color = color;
            }
            if (bulletName == "Mage"){
                Destroy(gameObject);
            }
            if (bulletName == "Archer"){
                Destroy(gameObject);
            }
        }

        if(other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Objects")) Destroy(gameObject);

    }


    private void BulletClass(string bulletName)
    {
        switch (bulletName)
        {
            case "Archer":
                print("Flecha");
                timeToDestroy = 3;
                break;
            case "Dagger":
                print("Slash");
                defaultX = .3f;
                defaultY = .3f;
                break;
            case "Mage":
                print("Bola de Fogo");
                timeToDestroy = 3;
                break;

        }

    }
}
