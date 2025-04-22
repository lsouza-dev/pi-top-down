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
    public List<Image> heatlthBarImages = new List<Image>();
    public float timeToDisappear = 5f;
    // Start is called before the first frame update

    void Awake()
    {
        lifeBar = GetComponent<Slider>();
        if (lifeBar == null) Debug.LogError("Slider component not found in children of Canvas.");
        heatlthBarImages = new List<Image>(GetComponentsInChildren<Image>());
    }

    void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (isActive)
        {
            
            gameObject.SetActive(true);
            this.transform.position = Camera.main.WorldToScreenPoint(this.enemy.transform.position + new Vector3(0, .3f, 0));
            lifeBar.value = enemy.currentHealth / enemy.maxHealth;
            LifeBarOpacityController();
        }
        else
        {
            gameObject.SetActive(false);
        }

        timeToDisappear -= Time.deltaTime;
    }

    public void LifeBarOpacityController()
    {
        if (timeToDisappear <= 0) heatlthBarImages.ForEach(h => h.color = new Color(h.color.r, h.color.g, h.color.b, .5f));
        else heatlthBarImages.ForEach(h => h.color = new Color(h.color.r, h.color.g, h.color.b, 1f));
    }
}
