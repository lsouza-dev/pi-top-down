using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioSource Effect;
    private bool hasPlayed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            Effect.Play();
            hasPlayed = true;
        }
    }
}
