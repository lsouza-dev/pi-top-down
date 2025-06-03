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
    [SerializeField] public BossController bossController;

    public List<Image> heatlthBarImages = new List<Image>();
    public float timeToDisappear = 5f;
    public float yOffset = .3f;
    [Header("Redimensionamento da Barra de Vida")]
    public Vector2 healthBarSize = new Vector2(100, 40);
    public float extraYOffset = -0.2f;

    private SpriteRenderer targetSpriteRenderer;

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
            Transform trackedTransform = null;
            float spriteHeight = 1f;

            if (enemyController != null)
            {
                trackedTransform = enemyController.transform;
                targetSpriteRenderer = enemyController.GetComponentInChildren<SpriteRenderer>();
                if (targetSpriteRenderer != null)
                    spriteHeight = targetSpriteRenderer.bounds.size.y;
                gameObject.SetActive(true);
                lifeBar.value = enemyController.currentHealth / enemyController.maxHealth;
                if (!enemyController.isAlive)
                {
                    Destroy(lifeBar.gameObject);
                }
            }
            else if (spawnerController != null)
            {
                trackedTransform = spawnerController.transform;
                targetSpriteRenderer = spawnerController.GetComponentInChildren<SpriteRenderer>();
                if (targetSpriteRenderer != null)
                    spriteHeight = targetSpriteRenderer.bounds.size.y;
                gameObject.SetActive(true);
                lifeBar.value = spawnerController.currentHealth / spawnerController.maxHealth;
            }
            else if (minionController != null)
            {
                trackedTransform = minionController.transform;
                targetSpriteRenderer = minionController.GetComponentInChildren<SpriteRenderer>();
                if (targetSpriteRenderer != null)
                    spriteHeight = targetSpriteRenderer.bounds.size.y;
                gameObject.SetActive(true);
                lifeBar.value = minionController.currentHealth / minionController.maxHealth;
                if (!minionController.isAlive)
                {
                    Destroy(lifeBar.gameObject);
                }
            }
            else if (bossController != null)
            {
                trackedTransform = bossController.transform;
                targetSpriteRenderer = bossController.GetComponentInChildren<SpriteRenderer>();
                if (targetSpriteRenderer != null)
                    spriteHeight = targetSpriteRenderer.bounds.size.y;
                gameObject.SetActive(true);
                lifeBar.value = bossController.currentHealth / bossController.maxHealth;
                if (!bossController.isAlive)
                {
                    Destroy(lifeBar.gameObject);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }

            // Calcula a posição acima da sprite
            if (trackedTransform != null)
            {
                float totalYOffset = spriteHeight / 2f + yOffset + extraYOffset;
                this.transform.position = Camera.main.WorldToScreenPoint(trackedTransform.position + new Vector3(0, totalYOffset, 0));
            }

            // Redimensiona a barra de vida
            RectTransform rt = GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = healthBarSize;
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
