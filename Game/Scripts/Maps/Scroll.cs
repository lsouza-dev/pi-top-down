using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public string OriginalTxt = "Texto em língua desconhecida...";
    public string TranslateTxt = "Agora você entende o que está escrito!";
    public bool CanInteract = false;
    public bool isLanguageScroll = false;
    private UIManager uiManager;
    public static Scroll instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (CanInteract && Input.GetKeyDown(KeyCode.Space) && isLanguageScroll)
        {
            if (uiManager.activatedPanel != null)
                return; // Se já há um painel aberto, não faz nada
            print("Aprendendo uma nova linguagem");
            uiManager.LanguageRead();
            LearnLanguage();
        }
        else if (CanInteract && Input.GetKeyDown(KeyCode.Space) && !isLanguageScroll)
        {
            if (uiManager.activatedPanel != null)
                return; // Se já há um painel aberto, não faz nada
            print("Abrindo pergaminho do boss...");
            OpenBossTxt();
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.CloseRead();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !gameObject.CompareTag("ScrollLang"))
        {
            CanInteract = true;
        }
        if (other.CompareTag("Player") && gameObject.CompareTag("ScrollLang"))
        {
            isLanguageScroll = true;
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanInteract = false;
            isLanguageScroll = false;
        }
    }

    void OpenBossTxt()
    {
        uiManager.BossRead();
        // Pegou o pergaminho .... UIManager.instance.haveLanguage = ...
    }
    public void LearnLanguage()
    {
        uiManager.haveLanguage = true;
        //PlayerPrefs.SetInt("TemLinguagem", 1);
        PlayerPrefs.Save();
        Debug.Log("Linguagem aprendida!");
    }
}