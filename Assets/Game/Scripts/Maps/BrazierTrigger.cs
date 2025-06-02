using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class BrazierTrigger : MonoBehaviour
{
    public ParticleSystem fireParticles; // Refer�ncia ao sistema de part�culas
    public List<Light2D> light2D;
    [SerializeField] private Sprite sprite;
    private SpriteRenderer spRender;

    private bool isBrazier;

    void Awake()
    {
        isBrazier = gameObject.name.Contains("Brazier");
        spRender = GetComponentInChildren<SpriteRenderer>();
    }
    void Start()
    {
        foreach (Light2D li in light2D)
        {
            li.gameObject.SetActive(!li.gameObject.activeSelf);
            if (li.gameObject.activeSelf) spRender.sprite = sprite;

            if (fireParticles != null)
            {
                fireParticles.Stop(); // Garante que come�a desativado
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica se o Player passou pelo collider
        {
            if (fireParticles != null)
            {
                if (!isBrazier)
                {
                    var spRender = GetComponentInChildren<SpriteRenderer>();
                    spRender.sprite = sprite;
                }                
                fireParticles.Play(); // Ativa as part�culas
                foreach (Light2D li in light2D)
                {
                    li.gameObject.SetActive(true);
                }           
            }
        }
    }
}
