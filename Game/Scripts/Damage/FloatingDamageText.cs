using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public float floatSpeed = 1f;
    public float fadeSpeed = 1f;

    public TMP_Text tmpText;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
    }
    
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        offset.y += .005f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;

        canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
        if (canvasGroup.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
