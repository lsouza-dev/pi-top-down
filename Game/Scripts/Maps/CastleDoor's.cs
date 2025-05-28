using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDoor : MonoBehaviour
{
    public Transform destination; // Outra porta para onde o jogador será enviado
    public float delay = 0.5f; // Tempo de espera antes do teleporte
    private bool isOpen = false; // Para evitar loops de teleporte

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpen) // Verifica se é o jogador e se não está teleportando
        {
            StartCoroutine(Teleport(other));
        }
    }

    IEnumerator Teleport(Collider2D player)
    {
        isOpen = true;
        yield return new WaitForSeconds(delay); // Espera antes de teleportar
        player.transform.position = destination.position;

        // Garante que o jogador não teleporte de volta imediatamente
        CastleDoor otherDoor = destination.GetComponent<CastleDoor>();
        if (otherDoor != null)
        {
            otherDoor.isOpen = true;
            yield return new WaitForSeconds(delay); // Pequeno delay antes de permitir novo teleporte
            otherDoor.isOpen = false;
        }

        isOpen = false;
    }
}