using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorTeleporter : MonoBehaviour
{
    [Header("Destino da porta")]
    public Transform destination;

    [Header("Deslocamento aplicado ao destino")]
    public Vector2 offset = new Vector2(-0.1f, -0.6f);

    private bool playerInRange = false;
    private bool canTeleport = true; // Controle de cooldown de teleporte

    private GameObject player;

    private void Update()
    {
        if (playerInRange && canTeleport && Input.GetKeyDown(KeyCode.F))
        {
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        if (destination == null || player == null) return;

        // Teleporta o jogador com offset
        player.transform.position = (Vector2)destination.position + offset;

        // Avisa a porta de destino para não teleportar imediatamente de volta
        DoorTeleporter destinationDoor = destination.GetComponent<DoorTeleporter>();
        if (destinationDoor != null)
        {
            destinationDoor.DisableTeleportTemporarily();
        }

        // Desabilita teleporte temporariamente nesta porta
        canTeleport = false;
    }

    // Usado pela porta de origem para desabilitar temporariamente o teleporte
    public void DisableTeleportTemporarily()
    {
        canTeleport = false;
        Invoke(nameof(EnableTeleport), 0.5f); // Reativa depois de meio segundo
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
            EnableTeleport(); // Garante que a porta possa ser usada novamente depois
        }
    }
}
