using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeleporter : MonoBehaviour
{
    public enum DoorUnlockCondition
    {
        None,
        AllSpawnersDestroyed,
        GotLanguageScroll,
        InteractedWithBossScroll,
        BossKilled,
    }

    [Header("Destino da porta (na mesma cena)")]
    public Transform destination;

    [Header("Deslocamento aplicado ao destino")]
    public Vector2 offset = new Vector2(-0.1f, -0.6f);

    [Header("Cena a carregar (deixe vazio se for teleporte local)")]
    public string SceneNames;

    [Header("Condição de desbloqueio da porta")]
    public DoorUnlockCondition unlockCondition = DoorUnlockCondition.None;

    // Lista de cenas válidas para verificação
    private readonly List<string> ValidScenes = new List<string>
    {
        "Desert",
        "Snow",
        "Swamp",
        "FinalBoss",
    };

    [SerializeField] private bool playerInRange = false;
    [SerializeField] private bool canTeleport = true;
    [SerializeField] private bool isUnlocked = false;

    private GameObject player;

    private void Update()
    {
        CheckUnlockCondition();

        if (playerInRange && canTeleport && isUnlocked && Input.GetKeyDown(KeyCode.Space))
        {
            print("pode telar");
            if (!string.IsNullOrEmpty(SceneNames))
            {
                LoadValidScene();
            }
            else
            {
                TeleportPlayer();
            }
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

            case DoorUnlockCondition.BossKilled:
                isUnlocked = GameManager.Instance.AreBossKilled();
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

    private void LoadValidScene()
    {
        if (ValidScenes.Contains(SceneNames))
        {
            SceneManager.LoadScene(SceneNames);
        }
        else
        {
            Debug.LogWarning($"Cena '{SceneNames}' não está na lista de cenas válidas!");
        }
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
