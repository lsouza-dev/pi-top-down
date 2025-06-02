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
            if(i == 0) uiAttributes[i].text = $"Atk. Speed: {playerController.atkSpeed}";
            if(i == 1) uiAttributes[i].text = $"Crít. Damage: {playerController.critDamage}%";
            if(i == 2) uiAttributes[i].text = $"Crít. Rate: {playerController.critRate}%";
            if(i == 3) uiAttributes[i].text = $"{playerController.currentHealth} / {playerController.maxHealth}";
            if(i == 4) uiAttributes[i].text = $"Level: {playerController.level}";
            if(i == 5) uiAttributes[i].text = $"Power: {playerController.strength}";
            if(i == 6) uiAttributes[i].text = $"Speed: {playerController.maxSpeed}";
            if(i == 7) uiAttributes[i].text = $"XP: {playerController.xp} / {playerController.nexLevelPoints}";
        }
    }
}
