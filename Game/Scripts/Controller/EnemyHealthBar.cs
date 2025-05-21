using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider lifeBar;
    public bool isActive = false;
    [SerializeField] public EnemyController enemyController;
    [SerializeField] public SpawnerController spawnerController;
    [SerializeField] public MinionController minionController;

    public List<Image> heatlthBarImages = new List<Image>();
    public float timeToDisappear = 5f;
    public float yOffset = .3f;
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
            if (enemyController != null)
            {
                gameObject.SetActive(true);
                this.transform.position = Camera.main.WorldToScreenPoint(this.enemyController.transform.position + new Vector3(0, yOffset, 0));
                lifeBar.value = enemyController.currentHealth / enemyController.maxHealth;

                if(!enemyController.isAlive){
                    Destroy(lifeBar.gameObject);
                }
            }
            else if(spawnerController != null){
                gameObject.SetActive(true);
                this.transform.position = Camera.main.WorldToScreenPoint(this.spawnerController.transform.position + new Vector3(0, yOffset, 0));
                lifeBar.value = spawnerController.currentHealth / spawnerController.maxHealth;
            }else if(minionController != null){
                gameObject.SetActive(true);
                this.transform.position = Camera.main.WorldToScreenPoint(this.minionController.transform.position + new Vector3(0, yOffset, 0));
                lifeBar.value = minionController.currentHealth / minionController.maxHealth;
            }
            else
            {
                gameObject.SetActive(false);
            }
           

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
