using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUpgradeController : MonoBehaviour
{

    [SerializeField] public List<TMP_Text> attributesCountText = new List<TMP_Text>();
    private int atkSpeed = 0;
    private int speed = 0;
    private int hp = 0;
    private int strength = 0;

    private int maxUpgrades = 3;
    private int upgrades = 0;

    [SerializeField] private GameObject fadeUpgradePannel;
    [SerializeField] private Button confirm;
    [SerializeField] private Button reset;
    [SerializeField] private List<Button> upgradeButtons = new List<Button>();


    // Start is called before the first frame update
    void Start()
    {
        fadeUpgradePannel = Resources.Load<GameObject>("Attributes/FadeUpgradePannel");
        upgradeButtons = GameObject.FindGameObjectsWithTag("UpgradeActionButton").OrderBy(a => a.name).Select(go => go.GetComponent<Button>()).ToList();
        attributesCountText = GameObject.FindGameObjectsWithTag("UpgradeCount").OrderBy(a => a.name).Select(go => go.GetComponent<TMP_Text>()).ToList();
        confirm = GameObject.FindWithTag("ConfirmUpgrade").GetComponent<Button>();
        reset = GameObject.FindWithTag("ResetUpgrade").GetComponent<Button>();
        confirm.interactable = false;
        SetActionsForButtons();
    }

    public void UpAtkSpeed()
    {
        if (upgrades < maxUpgrades)
        {
            attributesCountText[0].text = $"{atkSpeed += 1}";
            upgrades += 1;
            VerifyFinishButton();
        }
    }
    public void UpHp()
    {
        if (upgrades < maxUpgrades)
        {
            attributesCountText[1].text = $"{hp += 1}";
            upgrades += 1;
            VerifyFinishButton();
        }
    }
    public void UpSpeed()
    {
        if (upgrades < maxUpgrades)
        {
            attributesCountText[2].text = $"{speed += 1}";
            upgrades += 1;
            VerifyFinishButton();
        }
    }
    public void UpStrength()
    {
        if (upgrades < maxUpgrades)
        {
            attributesCountText[3].text = $"{strength += 1}";
            upgrades += 1;
            VerifyFinishButton();
        }
    }

    public void ResetCounters()
    {
        speed = 0;
        atkSpeed = 0;
        hp = 0;
        strength = 0;
        upgrades = 0;
        foreach (var t in attributesCountText)
        {
            t.text = "0";
        }
        confirm.interactable = false;
    }

    public void VerifyFinishButton()
    {
        if (upgrades == maxUpgrades)
        {
            confirm.interactable = true;
        }
    }

    public void OpenPanel()
    {
        var canva = GameObject.Find("Canvas");
        Instantiate(fadeUpgradePannel,canva.transform);
    }

    public void ClosePanel()
    {
        var player = FindObjectOfType<PlayerController>();
        var uiAttributesController = FindObjectOfType<UIAtributtesController>();
        Time.timeScale = 1f;

        player.maxHealth += hp * 10;
        player.maxSpeed += speed * .05f;
        player.strength += strength;
        player.atkSpeed += atkSpeed * .1f;
        player.currentHealth = player.maxHealth;

        uiAttributesController.SetAttributesValuesToUI();
        Destroy(this.gameObject);
    }

    public void SetActionsForButtons(){
        for (int i = 0; i < upgradeButtons.Count; i++) {
            if(i == 0) upgradeButtons[i].onClick.AddListener(UpAtkSpeed);
            if(i == 1) upgradeButtons[i].onClick.AddListener(UpHp);
            if(i == 2) upgradeButtons[i].onClick.AddListener(UpSpeed);
            if(i == 3) upgradeButtons[i].onClick.AddListener(UpStrength);
        }

        reset.onClick.AddListener(ResetCounters);
        confirm.onClick.AddListener(ClosePanel);
    }

}
