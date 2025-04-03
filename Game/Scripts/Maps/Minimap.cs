using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform target; // Alvo da câmera (ex: jogador)
    public Vector2 minBounds; // Limite mínimo da câmera (X, Y)
    public Vector2 maxBounds; // Limite máximo da câmera (X, Y)

    private Camera cam;
    private float camWidth, camHeight;

    void Start()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect; // Aspecto da tela
    }

    void LateUpdate()
    {
        if (target == null) return;

        float newX = Mathf.Clamp(target.position.x, minBounds.x + camWidth, maxBounds.x - camWidth);
        float newY = Mathf.Clamp(target.position.y, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(newX, newY, transform.position.z);
    }
}
