using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierTrigger : MonoBehaviour
{
    public ParticleSystem fireParticles; // Refer�ncia ao sistema de part�culas

    void Start()
    {
        if (fireParticles != null)
        {
            fireParticles.Stop(); // Garante que come�a desativado
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se o Player passou pelo collider
        {
            if (fireParticles != null)
            {
                fireParticles.Play(); // Ativa as part�culas
            }
        }
    }
}
