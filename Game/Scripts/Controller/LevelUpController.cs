using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpController : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;
    [SerializeField] private AttributeUpgradeController attributeUpgradeController;
    [SerializeField] private GameObject fadeUpgradePannel;


    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        attributeUpgradeController = FindObjectOfType<AttributeUpgradeController>();
        fadeUpgradePannel = Resources.Load<GameObject>("Attributes/FadeUpgradePannel");
    }


    public void LevelUp()
    {
        Time.timeScale = 0f;
        var canva = GameObject.Find("Canvas");
        Instantiate(fadeUpgradePannel, canva.transform);

        playerController.nexLevelPoints *= 2.5f;
        playerController.level += 1;

        switch (playerController.level)
        {
            case 2:
                playerController.EvolvePlayer(playerController.evolutionIndex, playerController.playerClass);
                playerController.maxHp += 30;
                playerController.currentHp = playerController.maxHp;
                playerController.atkSpeed += 0.5f;
                playerController.strength += 10;
                playerController.maxSpeed += 0.2f;

                break;
            case 3:
                playerController.EvolvePlayer(playerController.evolutionIndex, playerController.playerClass);
                playerController.maxHp += 50;
                playerController.currentHp = playerController.maxHp;
                playerController.atkSpeed += 0.8f;
                playerController.strength += 15;
                playerController.maxSpeed += 0.4f;
                break;
            case 4:
                playerController.EvolvePlayer(playerController.evolutionIndex, playerController.playerClass);
                playerController.maxHp += 100;
                playerController.currentHp = playerController.maxHp;
                playerController.atkSpeed += 1.2f;
                playerController.strength += 25;
                playerController.maxSpeed += .8f;
                break;
        }

    }

}
