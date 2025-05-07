using TMPro;
using UnityEngine;

public class DamageFeedbackController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject damageTextGO;
    [SerializeField] private PlayerController player;

    [SerializeField] public float xOffset = 1f;
    [SerializeField] public Color color;

    private bool isPlayer = false;
    private bool isSpawner = false;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        damageTextGO = Resources.Load<GameObject>("DamageText");
        player = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        isPlayer = !gameObject.name.Contains("(Clone)") && !gameObject.name.Contains("Spawner");
    }

    public void ShowDamageFeedback(float damageAmount)
    {
        var gmPos = isPlayer ? player.transform.position : transform.position;

        var instance = Instantiate(damageTextGO, canvas.transform);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(gmPos);
        instance.transform.position = screenPos;

        var floatingComponent = instance.GetComponent<FloatingDamageText>();
        if (floatingComponent == null) return;

        floatingComponent.tmpText.text = $"-{damageAmount}";
        floatingComponent.target = isPlayer ? player.transform : transform;
        floatingComponent.offset += new Vector3(xOffset, 0, 0);

        color = isPlayer ? Color.red : Color.white;
        
        floatingComponent.tmpText.color = color;

        var canvasGroup = instance.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }
}
