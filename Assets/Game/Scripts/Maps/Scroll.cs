using System.Collections;
using TMPro;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public string OriginalTxt = "Texto em língua desconhecida...";
    public string TranslateTxt = "Agora você entende o que está escrito!";
    public bool canInteract = false;
    public bool isLanguageScroll = false;

    [SerializeField] private AudioSource effect;
    [SerializeField] private GameObject actionTextGO;
    [SerializeField] private float yOffset = 1f; // Offset vertical do texto na tela
    [SerializeField] public bool isActiveActionTextGO = false;
    private UIManager uiManager;
    public static Scroll instance;

    // Animação
    [SerializeField] private float pulseSpeed = .25f;
    [SerializeField] private float pulseMutiplier = 5f;
    [SerializeField] private float scaleMultiplier = 0.2f;
    [SerializeField] private float alphaMin = 0.2f;
    private bool pulsing = false;
    private Coroutine pulseCoroutine;
    private TextMeshProUGUI actionTMP;
    private SpriteRenderer sprite;

    void Awake()
    {
        if (instance == null)
            instance = this;
         sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        actionTextGO.SetActive(false);
        uiManager = FindObjectOfType<UIManager>();
        actionTMP = actionTextGO.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (canInteract && isActiveActionTextGO && Input.GetKeyDown(KeyCode.F))
        {
            if (uiManager.activatedPanel != null) return;
            actionTextGO.SetActive(false);

            if (isLanguageScroll) LearnLanguage();
            else OpenBossTxt();

        }
        else if (canInteract && !isActiveActionTextGO && Input.GetKeyDown(KeyCode.F))
            uiManager.CloseRead();


        if (isActiveActionTextGO && actionTextGO != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, yOffset, 0));
            actionTextGO.transform.position = screenPos;
        }

        // sprite.gameObject.SetActive(true);
        // sprite.gameObject.transform.position = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            if (gameObject.CompareTag("ScrollLang"))
                isLanguageScroll = true;

            isActiveActionTextGO = true;
            actionTextGO.SetActive(isActiveActionTextGO);

            pulsing = true;
            pulseCoroutine = StartCoroutine(PulseText());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            isLanguageScroll = false;
            isActiveActionTextGO = false;
            actionTextGO.SetActive(isActiveActionTextGO);

            pulsing = false;
            if (pulseCoroutine != null)
                StopCoroutine(pulseCoroutine);
        }
    }

    IEnumerator PulseText()
    {
        Vector3 baseScale = actionTextGO.transform.localScale;

        while (pulsing)
        {
            float scale = 1 + Mathf.PingPong(Time.time * pulseSpeed, scaleMultiplier);
            float alpha = Mathf.Lerp(alphaMin, 1f, Mathf.PingPong(Time.time * (pulseSpeed * pulseMutiplier), 1f));

            actionTextGO.transform.localScale = baseScale * scale;

            if (actionTMP != null)
            {
                Color c = actionTMP.color;
                c.a = alpha;
                actionTMP.color = c;
            }

            yield return null;
        }

        // Reset
        if (actionTMP != null)
        {
            Color c = actionTMP.color;
            c.a = 1f;
            actionTMP.color = c;
        }

        actionTextGO.transform.localScale = baseScale;
    }

    void OpenBossTxt()
    {
        isActiveActionTextGO = false;
        effect.Play();
        Time.timeScale = 0f; // Pausa o jogo
        uiManager.BossRead();
    }


    public void LearnLanguage()
    {
        isActiveActionTextGO = false;
        uiManager.haveLanguage = true;
        effect.Play();
        Time.timeScale = 0f; // Pausa o jogo
        uiManager.LanguageRead();
        GameManager.Instance.CollectScroll("Language");
    }
}