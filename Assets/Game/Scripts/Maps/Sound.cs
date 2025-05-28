using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomSound : MonoBehaviour
{
    public AudioSource bossMusic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!bossMusic.isPlaying)
            {
                bossMusic.Play();
            }
        }
    }
}