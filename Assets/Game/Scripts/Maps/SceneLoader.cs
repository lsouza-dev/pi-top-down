using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string targetScene;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("colidiu aqui ó");
            LoadTargetScene();
        }
    }
    public void LoadTargetScene()
    {
        PlayerPrefs.SetString("TargetScene", targetScene);
        SceneManager.LoadScene("Loading Scene");
    }
}
