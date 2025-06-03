using System.Collections;
using UnityEngine;

public class DoorTeleporter : MonoBehaviour
{
    public enum DoorUnlockCondition
    {
        None,
        AllSpawnersDestroyed,
        GotLanguageScroll,
        InteractedWithBossScroll
    }

    [Header("Destino da porta")]
    public Transform destination;

    [Header("Deslocamento aplicado ao destino")]
    public Vector2 offset = new Vector2(-0.1f, -0.6f);

    [Header("Condição de desbloqueio da porta")]
    public DoorUnlockCondition unlockCondition = DoorUnlockCondition.None;

    private bool playerInRange = false;
    private bool canTeleport = true;
    private bool isUnlocked = false;

    private GameObject player;

    private void Update()
    {
        CheckUnlockCondition();

        if (playerInRange && canTeleport && isUnlocked && Input.GetKeyDown(KeyCode.Space))
        {
            TeleportPlayer();
        }
    }

    private void CheckUnlockCondition()
    {
        if (GameManager.Instance == null)
        {
            isUnlocked = false;
            return;
        }

        switch (unlockCondition)
        {
            case DoorUnlockCondition.None:  
                isUnlocked = true;
                break;

            case DoorUnlockCondition.AllSpawnersDestroyed:
                isUnlocked = GameManager.Instance.AreAllSpawnersDestroyed();
                break;

            case DoorUnlockCondition.GotLanguageScroll:
                isUnlocked = GameManager.Instance.HasScroll("Language");
                break;

            case DoorUnlockCondition.InteractedWithBossScroll:
                isUnlocked = GameManager.Instance.HasScroll("Language") &&
                             GameManager.Instance.HasInteractedWithBossScroll();
                break;
        }
    }

    private void TeleportPlayer()
    {
        if (destination == null || player == null) return;

        player.transform.position = (Vector2)destination.position + offset;

        DoorTeleporter destinationDoor = destination.GetComponent<DoorTeleporter>();
        if (destinationDoor != null)
        {
            destinationDoor.DisableTeleportTemporarily();
        }

        canTeleport = false;
    }

    public void DisableTeleportTemporarily()
    {
        canTeleport = false;
        Invoke(nameof(EnableTeleport), 0.5f);
    }

    private void EnableTeleport()
    {
        canTeleport = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            EnableTeleport();
        }
    }
}
