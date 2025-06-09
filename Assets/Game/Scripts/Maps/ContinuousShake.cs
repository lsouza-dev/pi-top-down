using UnityEngine;
using Cinemachine;

public class ContinuousShake : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    private bool isShaking = false;
    private float actPos;

    public GameObject player;

    public void StartShake()
    {
        isShaking = true;
    }

    public void StopShake()
    {
        isShaking = false;
    }

    void Update()
    {
        Vector3 actPos = player.transform.position;
        if (isShaking)
        {
            impulseSource.GenerateImpulseAt(actPos, Vector3.right * Random.Range(-0.05f, 0.05f) + Vector3.up * Random.Range(0.05f, -0.05f));
        }
    }
}