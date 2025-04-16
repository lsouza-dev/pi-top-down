using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider lifeBar;
    public bool isActive = false;
    [SerializeField] public EnemyController enemy;
    // Start is called before the first frame update

    void Awake()
    {
        lifeBar = GetComponent<Slider>();

        if (lifeBar == null) Debug.LogError("Slider component not found in children of Canvas.");        
        // if(enemy == null) Debug.LogError("EnemyController component not found in children of Canvas. In the Object: " + gameObject.name);
    }

    void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if(isActive){
            gameObject.SetActive(true);
            this.transform.position = Camera.main.WorldToScreenPoint(this.enemy.transform.position + new Vector3(0, .3f, 0));
            lifeBar.value = enemy.currentHealth / enemy.maxHealth;
        }else{
            gameObject.SetActive(false);
        }
    }
}
