using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform destination; // Outra porta para onde o jogador será enviado
    public float delay = 0.4f; // Tempo de espera antes do teleporte
    public float offsetY = -0.6f; // Valor para mover o player em Y em relação à porta de destino
    public float offsetX = -0.1f; // Valor para mover o player em Y em relação à porta de destino
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

        // Teleporta o jogador para a posição da porta de destino, com o offsetY aplicado
        Vector3 teleportPosition = destination.position;
        teleportPosition.y += offsetY; // Aplica o deslocamento em Y
        teleportPosition.x += offsetX;
        player.transform.position = teleportPosition;

        // Garante que o jogador não teleporte de volta imediatamente
        Doors otherDoor = destination.GetComponent<Doors>();
        if (otherDoor != null)
        {
            otherDoor.isOpen = true;
            yield return new WaitForSeconds(delay); // Pequeno delay antes de permitir novo teleporte
            otherDoor.isOpen = false;
        }

        isOpen = false;
    }
}