using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomSound : MonoBehaviour
{
    public AudioSource Music;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            print("TriggerEnter");
            if (!Music.isPlaying)
            {
                print("Tocando");
                Music.Play();
            }
            else
            {
                Music.UnPause();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("TriggerExit");
            if (Music.isPlaying)
            {
            print("parou");
                Music.Pause();
            }
        }
    }
}