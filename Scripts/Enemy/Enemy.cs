using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health = 5f;
    [SerializeField] float collisionDamage = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage){
        this.health -= damage;
        if(this.health <= 0){
            Destroy(this.gameObject);
        }
    }

    public void PlayerHit(PlayerController player){
        player.TakeDamage(collisionDamage);
    }
}
