using TMPro;
using UnityEngine;

public class DamageFeedbackController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject damageTextGO;
    [SerializeField] private PlayerController player;

    [SerializeField] public float xOffset = 1f;

    private bool isPlayer = false;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        damageTextGO = Resources.Load<GameObject>("DamageText");
        player = FindObjectOfType<PlayerController>();
    }

    void Start()
    {
        isPlayer = !gameObject.name.Contains("(Clone)");
    }

    public void ShowDamageFeedback(float damageAmount)
    {
        var gmPos = isPlayer ? player.transform.position : transform.position;

        // Instancia o texto no canvas
        var instance = Instantiate(damageTextGO, canvas.transform);

        // Converte para posição de tela corretamente
        Vector3 worldPos = gmPos;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // Aplica a posição na tela com base no Canvas
        instance.transform.position = screenPos;

        // Inicializa texto e comportamento
        var floatingText = instance.GetComponent<FloatingDamageText>();

        floatingText.target = isPlayer ? player.transform : transform;
        floatingText.offset += new Vector3(xOffset, 0, 0);
        floatingText.SetText($"-{damageAmount}");
        print($"Object: - {gameObject.name} - PLAYER??? {isPlayer}");
        var color = isPlayer ? Color.red : Color.white;
        floatingText.SetColor(color);
    }
}
