using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomSound : MonoBehaviour
{
    public AudioSource Music;
    public static BossRoomSound Instance;


    public void Awake()
    {
        Instance = this;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (!Music.isPlaying)
            {
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
            if (Music.isPlaying)
            {
                Music.Pause();
            }
        }
    }
    public void MusicPause()
    {
        Music.Pause();
    }
}