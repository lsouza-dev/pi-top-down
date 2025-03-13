using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 5f;
    [SerializeField] float collisionDamage = 5f;
    [SerializeField] LevelUpController levelUpController;
    // Start is called before the first frame update
    void Awake()
    {
        levelUpController = FindObjectOfType<LevelUpController>();
    }


    public void TakeDamage(float damage){
        this.health -= damage;
        if(this.health <= 0){
            var player = FindObjectOfType<PlayerController>();
            player.xp += 20;
            if(player.xp >= player.nexLevelPoints) levelUpController.LevelUp();
            Destroy(this.gameObject);
        }


    }

    public void PlayerHit(PlayerController player){
        player.TakeDamage(collisionDamage);
    }
}
