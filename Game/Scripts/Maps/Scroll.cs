using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public string OriginalTxt = "Texto em língua desconhecida...";
    public string TranslateTxt = "Agora você entende o que está escrito!";
    public bool CanInteract = false;
    public bool isLanguageScroll = false;
    [SerializeField] private GameObject actionTextGO;
    private bool isActiveActionTextGO = false;
    private UIManager uiManager;
    public static Scroll instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        actionTextGO = GameObject.FindGameObjectWithTag("ActionText");
    }

    private void Start()
    {
        actionTextGO.SetActive(false);
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            if (uiManager.activatedPanel != null) return;

            if (isLanguageScroll)
            {
                Debug.Log("Aprendendo uma nova linguagem");
                uiManager.LanguageRead();
                LearnLanguage();
            }
            else
            {
                Debug.Log("Abrindo pergaminho do boss...");
                OpenBossTxt();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F) && !isActiveActionTextGO)
        {
            uiManager.CloseRead();
        }
    }

    void LateUpdate()
    {
        if (isActiveActionTextGO && actionTextGO != null)
        {
            Vector3 offset = new Vector3(0, 1.5f, 0);
            Vector3 worldPos = transform.position + offset;

            // Converte posição do mundo para posição de tela
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            // [Opcional] Limita para manter dentro da tela, caso haja exagero de posição
            screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
            screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);

            // Posiciona o UI diretamente
            RectTransform actionRect = actionTextGO.GetComponent<RectTransform>();
            actionRect.position = screenPos;
        }
    }






    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            CanInteract = true;
            if (gameObject.CompareTag("ScrollLang"))
                isLanguageScroll = true;

            actionTextGO.SetActive(true);
            isActiveActionTextGO = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteract = false;
            isLanguageScroll = false;
            actionTextGO.SetActive(false);
            isActiveActionTextGO = true;
        }
    }

    void OpenBossTxt()
    {
        isActiveActionTextGO = false;
        uiManager.BossRead();
        // Pegou o pergaminho .... UIManager.instance.haveLanguage = ...
    }
    public void LearnLanguage()
    {
        isActiveActionTextGO = false;
        uiManager.haveLanguage = true;
        //PlayerPrefs.SetInt("TemLinguagem", 1);
        PlayerPrefs.Save();
        Debug.Log("Linguagem aprendida!");
    }
}