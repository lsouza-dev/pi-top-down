using UnityEngine;

public class BossWakeUp : MonoBehaviour
{
    public AudioSource roar;
    public ContinuousShake shakeController;
    public AudioSource Music;
    public PlayerController playerController;

    private bool hasTriggered = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(WakeUpSequence());
        }
    }

    private System.Collections.IEnumerator WakeUpSequence()
    {
        Music.Pause();
        yield return new WaitForSeconds(0.8f);
        roar.Play();
        shakeController.StartShake();

        yield return new WaitForSecondsRealtime(roar.clip.length);

        shakeController.StopShake();
        yield return new WaitForSeconds(0.8f);
        Music.UnPause();
    }
}
