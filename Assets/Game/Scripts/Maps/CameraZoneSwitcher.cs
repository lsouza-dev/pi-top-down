using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraZoneSwitcher : MonoBehaviour
{
    [Header("Confiner GameObjects")]
    public GameObject forestConfinerObj;
    public GameObject anteRoomConfinerObj;
    public GameObject bossRoomConfinerObj;

    [Header("Cinemachine Components")]
    public CinemachineConfiner confiner;
    public CinemachineVirtualCamera virtualCamera;

    [Header("Zoom Settings")]
    public float forestZoom = 6f;
    public float anteRoomZoom = 4f;
    public float bossRoomZoom = 12f;

    [Header("Camera Targets")]
    public Transform playerTransform;            // Referência ao jogador
    public Transform bossRoomFocusPoint;         // Ponto fixo para focar na bossRoom

    private void Awake()
    {
        // Se não setado manualmente, tenta encontrar automaticamente
        if (virtualCamera == null)
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (confiner == null)
            confiner = FindObjectOfType<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        switch (gameObject.name)
        {
            case "darkForestZone":
                ActivateZone(forestConfinerObj, forestZoom, playerTransform);
                break;

            case "anteRoomZone":
                ActivateZone(anteRoomConfinerObj, anteRoomZoom, playerTransform);
                break;

            case "bossRoomZone":
                ActivateZone(bossRoomConfinerObj, bossRoomZoom, bossRoomFocusPoint);
                break;
        }
    }

    void ActivateZone(GameObject targetConfinerObj, float targetZoom, Transform newFollowTarget)
    {
        // Desativa todos os confiners
        forestConfinerObj.SetActive(false);
        anteRoomConfinerObj.SetActive(false);
        bossRoomConfinerObj.SetActive(false);

        // Ativa o novo confiner
        targetConfinerObj.SetActive(true);

        // Atualiza o Collider do confiner
        Collider2D newCollider = targetConfinerObj.GetComponent<Collider2D>();
        confiner.m_BoundingShape2D = newCollider;

        // Atualiza a câmera
        if (virtualCamera != null)
        {
            StartCoroutine(SmoothZoom(targetZoom));

            if (newFollowTarget != null)
                virtualCamera.Follow = newFollowTarget;
        }

        Debug.Log("Zona ativada: " + gameObject.name);
    }

    IEnumerator SmoothZoom(float targetZoom)
    {
        float duration = 0.5f;
        float startZoom = virtualCamera.m_Lens.OrthographicSize;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startZoom, targetZoom, time / duration);
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetZoom;
    }
}
