using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referência ao player
    public float smoothSpeed = 5f;
    public Vector3 offset; // Offset da câmera em relação ao player

    void LateUpdate()
    {
        if (target == null) return;

        // Posição desejada com o offset
        Vector3 desiredPosition = target.position + offset;
        // Suaviza o movimento da câmera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
