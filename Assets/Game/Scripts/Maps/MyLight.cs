using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MyLight : MonoBehaviour
{

    public Light2D light2D;
    public float intensytiMin;
    public float intensytiMax;
    public float oscilateSpeed;

    private void Awake()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }
    }
    void Update()
    {
        float t = Mathf.PingPong(Time.time * oscilateSpeed, 1);
        light2D.intensity = Mathf.Lerp(intensytiMin, intensytiMax, t);
    }
}
