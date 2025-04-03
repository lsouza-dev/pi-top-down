using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIAtributtesController : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> uiAttributes = new List<TMP_Text>();
    private PlayerController playerController;

    
    // Start is called before the first frame update
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        uiAttributes = GameObject.FindGameObjectsWithTag("Attributes").OrderBy(a => a.name).Select(go => go.GetComponent<TMP_Text>()).ToList();       
        
    }

    public void SetAttributesValuesToUI(){
        for (int i = 0; i < uiAttributes.Count; i++) {
            if(i == 0) uiAttributes[i].text = playerController.atkSpeed.ToString();
            if(i == 1) uiAttributes[i].text = $"{playerController.currentHp} / {playerController.maxHp}";
            if(i == 2) uiAttributes[i].text = $"Lv: {playerController.level}";
            if(i == 3) uiAttributes[i].text = playerController.maxSpeed.ToString();
            if(i == 4) uiAttributes[i].text = playerController.strength.ToString();
            if(i == 5) uiAttributes[i].text = $"{playerController.xp} / {playerController.nexLevelPoints}";
        }
    }
}
