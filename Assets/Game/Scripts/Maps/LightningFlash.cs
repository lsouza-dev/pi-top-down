    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningFlash : MonoBehaviour
{
    public Light2D globalLight;
    public Color flashColor = Color.white;
    public float flashDuration;
    public float timeBetweenFlashes;

    private Color originalColor;
    [SerializeField] private AudioSource thunderAudio;
    [SerializeField] private AudioClip[] thunderSounds;

    void Start()
    {
        if (globalLight != null)
        {
            originalColor = globalLight.color;
            StartCoroutine(LightningRoutine());
        }
    }
    public IEnumerator LightningRoutine()
    {
        yield return new WaitForSeconds(Random.Range(2f, 6f));

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            yield return StartCoroutine(Flash());
            yield return new WaitForSeconds(timeBetweenFlashes);
            yield return StartCoroutine(Flash());
            yield return new WaitForSeconds(1.3f);
            PlayRandomThunder();
        }
    }
    private IEnumerator Flash()
    {
        globalLight.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        globalLight.color = originalColor;
    }

    void PlayRandomThunder()
    {
        AudioClip clip = thunderSounds[Random.Range(0, thunderSounds.Length)];
        thunderAudio.PlayOneShot(clip);
    }
}
