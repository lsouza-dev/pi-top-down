using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeBar : MonoBehaviour
{
    [SerializeField] private Slider lifeBar;
    private PlayerController player;
    // Start is called before the first frame update

    void Awake()
    {
        lifeBar = GetComponent<Slider>();
        player = FindObjectOfType<PlayerController>();

        if (lifeBar == null) Debug.LogError("Slider component not found in children of Canvas.");
        if (player == null) Debug.LogError("PlayerController component not found in children of Canvas.");
    }

    // Update is called once per frame
    void Update()
    {

        lifeBar.value = PlayerController.instance.currentHealth / PlayerController.instance.maxHealth;

    }
}
