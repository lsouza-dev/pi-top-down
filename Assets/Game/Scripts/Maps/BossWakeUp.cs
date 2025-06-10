using UnityEngine;

public class BossWakeUp : MonoBehaviour
{
    public AudioSource roar;
    public ContinuousShake shakeController;
    public AudioSource Music;
    public PlayerController playerController;

    private bool hasTriggered = false;

    private void Awake()
    {
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player") && GameManager.Instance.HasInteractedWithBossScroll())
        {
            hasTriggered = true;
            StartCoroutine(WakeUpSequence());
        }
    }

    private System.Collections.IEnumerator WakeUpSequence()
    {
        print("funcionou");
        playerController.FreezePlayer(10000f);
        Music.Pause();
        yield return new WaitForSeconds(0.8f);
        roar.Play();
        shakeController.StartShake();

        yield return new WaitForSecondsRealtime(roar.clip.length);

        shakeController.StopShake();
        yield return new WaitForSeconds(0.8f);
        Music.UnPause();

        playerController.FreezePlayer(0f);
    }
}
