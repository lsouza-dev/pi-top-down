using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Refer�ncia ao player
    public float smoothSpeed = 5f;
    public Vector3 offset; // Offset da c�mera em rela��o ao player

    void LateUpdate()
    {
        if (target == null) return;

        // Posi��o desejada com o offset
        Vector3 desiredPosition = target.position + offset;
        // Suaviza o movimento da c�mera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
