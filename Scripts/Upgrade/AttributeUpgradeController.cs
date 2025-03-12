using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Attr : MonoBehaviour
{

    [SerializeField] public List<TMP_Text> attributesCountText = new List<TMP_Text>();
    private int atkSpeed = 0;
    private int def = 0;
    private int hp = 0;
    private int strength = 0;

    private int maxUpgrades = 3;
    private int upgrades = 0;

    private GameObject upgradePanel;
    [SerializeField] private Button confirm;


    // Start is called before the first frame update
    void Start()
    {
        confirm = GameObject.FindWithTag("ConfirmUpgrade").GetComponent<Button>();
        confirm.interactable = false;
        attributesCountText = GameObject.FindGameObjectsWithTag("UpgradeCount").OrderBy(a => a.name).Select(go => go.GetComponent<TMP_Text>()).ToList();  
        upgradePanel = GameObject.Find("UpgradePanel");
    }

    public void UpAtkSpeed(){
        if(upgrades < maxUpgrades) {
            attributesCountText[0].text = $"x{atkSpeed+=1}";
           upgrades+= 1; 
           VerifyFinishButton();
        }
    }
    public void UpDef(){
       if(upgrades < maxUpgrades) {
            attributesCountText[1].text = $"x{def+=1}";
           upgrades+= 1; 
           VerifyFinishButton();
        }
    }
    public void UpHp(){
       if(upgrades < maxUpgrades) {
            attributesCountText[2].text = $"x{hp+=1}";
           upgrades+= 1; 
           VerifyFinishButton();
        }
    }
    public void UpStrength(){
       if(upgrades < maxUpgrades) {
            attributesCountText[3].text = $"x{strength+=1}";
           upgrades+= 1; 
           VerifyFinishButton();
        }
    }

    public void ResetCounters()
    {
        def = 0;
        atkSpeed = 0;
        hp = 0;
        strength = 0;
        upgrades = 0;
        foreach (var t in attributesCountText){
            t.text = "x0";
        }
        confirm.interactable = false;
    }

    public void VerifyFinishButton(){
        if(upgrades == maxUpgrades){
            confirm.interactable = true;
        }
    }

    public void ClosePanel(){
        var player = GameObject.FindObjectOfType<PlayerController>();
        player.currentHp += hp * 10;
        player.def += def * 1.1f;
        // player.maxSpeed +=  * 10;
        player.currentHp += hp * 10;
        upgradePanel.SetActive(false);
    }
}
