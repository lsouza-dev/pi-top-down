using UnityEngine;

public class Bullet : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] float timeToDestroy = .5f;
    [SerializeField] float damage = 1f;
    [SerializeField] float defaultX = .3f;
    [SerializeField] float defaultY = .3f;
    [SerializeField] float incrementValue = .01f;
    [SerializeField] float scaleX;
    [SerializeField] float scaleY;

    [Header("Compontents")]
    [SerializeField] SpriteRenderer sr;


    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
        transform.localScale = new Vector2(defaultX,defaultY);
        scaleX = defaultX;
        scaleY = defaultY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(scaleX, scaleY);
        scaleX += incrementValue;
        scaleY += incrementValue;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            
            var Enemy = other.gameObject.GetComponent<Enemy>();
            Enemy.TakeDamage(damage);

            damage *= .90f;
            Color color = sr.color;
            if(color.a <= 0) Destroy(gameObject);

            color.a = Mathf.Clamp01(color.a - .15f);
            sr.color = color;
        }
    }
}
