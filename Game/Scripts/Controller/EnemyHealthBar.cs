using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider lifeBar;
    public bool isActive = false;
    [SerializeField] private EnemyController enemy;
    public static EnemyHealthBar instance;
    // Start is called before the first frame update

    void Awake()
    {
        instance = instance == null ? this : instance;
        lifeBar = GetComponent<Slider>();
        enemy = GetComponentInParent<EnemyController>();

        if (lifeBar == null) Debug.LogError("Slider component not found in children of Canvas.");        
        if(enemy == null) Debug.LogError("EnemyController component not found in children of Canvas. In the Object: " + gameObject.name);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            gameObject.SetActive(true);
            print("Barra do inimigo ativada no " + enemy.gameObject.name);
            this.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, .3f, 0));
            lifeBar.value = enemy.currentHealth / enemy.maxHealth;
        }else{
            print("Barra do inimigo desativada!!! Enemy: " + enemy.gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
