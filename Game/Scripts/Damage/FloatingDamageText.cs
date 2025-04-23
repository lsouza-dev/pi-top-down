using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public float floatSpeed = 1f;
    public float fadeSpeed = 1f;

    private TMP_Text text;
    private CanvasGroup canvasGroup;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Sobe o texto com offset incremental
        offset.y += .005f;

        // Converte para posição de tela
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;

        // Fade out
        canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
        if (canvasGroup.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string value)
    {
        if (text != null)
            text.text = value;
    }

    public void SetColor(Color color)
    {
        if (text != null)
            text.color = color;
    }
}
