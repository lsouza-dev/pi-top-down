using UnityEngine;
using TMPro;

public class LoadingTextAnimator : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    private string baseText = "Loading";
    private float timer;
    private int dotCount;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.5f)
        {
            dotCount = (dotCount + 1) % 4;
            loadingText.text = baseText + new string('.', dotCount);
            timer = 0f;
        }
    }
}