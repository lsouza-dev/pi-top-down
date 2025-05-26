using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public GameObject activatedPanel; // Painel do pergaminho ativado     
    [SerializeField] private Canvas canvas;
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
        if (!haveLanguage)
        {
            switch (index)
            {
                case 0:
                    activatedPanel = Instantiate(abissalEncript, canvas.transform);
                    break;
                case 1:
                    activatedPanel = Instantiate(draconicEncript, canvas.transform);
                    break;
                case 2:
                    activatedPanel = Instantiate(primordialEncript, canvas.transform);
                    break;
                case 3:
                    activatedPanel = Instantiate(silvestreEncript, canvas.transform);
                    break;
                case 4:
                    activatedPanel = Instantiate(subterraneaEncript, canvas.transform);
                    break;

            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    activatedPanel = Instantiate(abissalTranslate, canvas.transform);
                    break;
                case 1:
                    activatedPanel = Instantiate(draconicTranslate, canvas.transform);
                    break;
                case 2:
                    activatedPanel = Instantiate(primordialTranslate, canvas.transform);
                    break;
                case 3:
                    activatedPanel = Instantiate(silvestreTranslate, canvas.transform);
                    break;
                case 4:
                    activatedPanel = Instantiate(subterraneaTranslate, canvas.transform);
                    break;
            }
        }
    }
    public void LanguageRead()
    {
        var index = 3;
        switch (index)
        {
            case 0:
                activatedPanel = Instantiate(languageAbissal, canvas.transform);
                break;
            case 1:
                activatedPanel = Instantiate(languageDraconic, canvas.transform);
                break;
            case 2:
                activatedPanel = Instantiate(languagePrimordial, canvas.transform);
                break;
            case 3:
                activatedPanel = Instantiate(languageSilvestre, canvas.transform);
                break;
            case 4:
                activatedPanel = Instantiate(languageUndercommon, canvas.transform);
                break;
        }
    }

    // Fecha o painel de leitura do pergaminho
    public void CloseRead()
    {
        Destroy(activatedPanel);
        Scroll.instance.isActiveActionTextGO = false;
        Time.timeScale = 1f; // Retoma o jogo
    }
}