using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public GameObject activatedPanel; // Painel do pergaminho ativado
    [SerializeField] public bool haveLanguage = false; // Painel do pergaminho ativado

    [Header("Encripted Scrolls")]
    [SerializeField] private GameObject abissalEncript;
    [SerializeField] private GameObject draconicEncript;
    [SerializeField] private GameObject primordialEncript;
    [SerializeField] private GameObject silvestreEncript;
    [SerializeField] private GameObject subterraneaEncript;

    [Header("Translated Scrolls")]
    [SerializeField] private GameObject abissalTranslate;
    [SerializeField] private GameObject draconicTranslate;
    [SerializeField] private GameObject primordialTranslate;
    [SerializeField] private GameObject silvestreTranslate;
    [SerializeField] private GameObject subterraneaTranslate;

    [Header("Language Scrolls")]
    [SerializeField] private GameObject languageAbissal;
    [SerializeField] private GameObject languageDraconic;
    [SerializeField] private GameObject languagePrimordial;
    [SerializeField] private GameObject languageSilvestre;
    [SerializeField] private GameObject languageUndercommon;

    public TextMeshProUGUI scrollText; // Componente de texto para exibir o conte�do

    public static UIManager instance;

    void Awake()
    {
        instance = instance == null ? this : instance;

        abissalEncript = Resources.Load<GameObject>("Encrip\\AbissalEncrip");
        draconicEncript = Resources.Load<GameObject>("Encrip\\DraconicEncrip");
        primordialEncript = Resources.Load<GameObject>("Encrip\\PrimordialEncrip");
        silvestreEncript = Resources.Load<GameObject>("Encrip\\SilvestreEncrip");
        subterraneaEncript = Resources.Load<GameObject>("Encrip\\UndercommonEncrip");
        
        
        abissalTranslate = Resources.Load<GameObject>("Resolv\\AbissalResolv");
        draconicTranslate = Resources.Load<GameObject>("Resolv\\DraconicResolv");
        primordialTranslate = Resources.Load<GameObject>("Resolv\\PrimordialResolv");
        silvestreTranslate = Resources.Load<GameObject>("Resolv\\SilvestreResolv");
        subterraneaTranslate = Resources.Load<GameObject>("Resolv\\UndercommonResolv");

        languageAbissal = Resources.Load<GameObject>("LanguageAbissal");
        languageDraconic = Resources.Load<GameObject>("LanguageDraconic");
        languagePrimordial = Resources.Load<GameObject>("LanguagePrimordial");
        languageSilvestre = Resources.Load<GameObject>("LanguageSilvestre");
        languageUndercommon = Resources.Load<GameObject>("LanguageUndercommon");

    }
    public void BossRead()
    {
        var index = 3;
        var canva = GameObject.Find("Canvas");
        Time.timeScale = 0f; // Pausa o jogo enquanto o pergaminho est� aberto
        Scroll.instance.CanInteract = false;

        if (!haveLanguage)
        {
            switch (index)
            {
                case 0:
                    activatedPanel = Instantiate(abissalEncript, canva.transform);
                    break;
                case 1:
                    activatedPanel = Instantiate(draconicEncript, canva.transform);
                    break;
                case 2:
                    activatedPanel = Instantiate(primordialEncript, canva.transform);
                    break;
                case 3:
                    activatedPanel = Instantiate(silvestreEncript, canva.transform);
                    break;
                case 4:
                    activatedPanel = Instantiate(subterraneaEncript, canva.transform);
                    break;

            }
        }else
        {
            switch (index)
            {
                case 0:
                    activatedPanel = Instantiate(abissalTranslate, canva.transform);
                    break;
                case 1:
                    activatedPanel = Instantiate(draconicTranslate, canva.transform);
                    break;
                case 2:
                    activatedPanel = Instantiate(primordialTranslate, canva.transform);
                    break;
                case 3:
                    activatedPanel = Instantiate(silvestreTranslate, canva.transform); 
                    break;
                case 4:
                    activatedPanel = Instantiate(subterraneaTranslate, canva.transform);
                    break;
            }
        }
    }
   public void LanguageRead()
   {
        var index = 3;
        var canva = GameObject.Find("Canvas");
        Scroll scroll = FindObjectOfType<Scroll>();
        Scroll.instance.CanInteract = false;
        Time.timeScale = 0f; // Pausa o jogo enquanto o pergaminho est� aberto

      switch (index)
            {
                case 0:
                    activatedPanel = Instantiate(languageAbissal, canva.transform);
                    break;
                case 1:
                    activatedPanel = Instantiate(languageDraconic, canva.transform);
                    break;    
                case 2:
                    activatedPanel = Instantiate(languagePrimordial, canva.transform);
                    break;
                case 3:
                    activatedPanel = Instantiate(languageSilvestre, canva.transform);
                    break;
                case 4:
                    activatedPanel = Instantiate(languageUndercommon, canva.transform);
                    break;
      }
   } 

    // Fecha o painel de leitura do pergaminho
    public void CloseRead()
    {
        Destroy(activatedPanel);
        Time.timeScale = 1f; // Retoma o jogo
    }
}