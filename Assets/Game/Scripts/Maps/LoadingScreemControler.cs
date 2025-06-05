using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public Image loadingImage;
    public Sprite florestaSprite;
    public Sprite pantanoSprite;
    public Sprite neveSprite;
    public Sprite desertoSprite;
    public Sprite bossSprite;

    void Awake()
    {
        loadingImage = GetComponent<Image>();
    }
    void Start()
    {
        string targetScene = PlayerPrefs.GetString("TargetScene");
        string loadingKey = "Loading_" + targetScene;

        switch (targetScene)
        {
            case "Forest":
                loadingImage.sprite = florestaSprite;
                break;
            case "Swamp":
                loadingImage.sprite = pantanoSprite;
                break;
            case "Snow":
                loadingImage.sprite = neveSprite;
                break;
            case "Desert":
                loadingImage.sprite = desertoSprite;
                break;
            case "FinalBoss":
                loadingImage.sprite = bossSprite;
                break;
            default:
                print("cena nao encontrada");
                break;
        }
        
        StartCoroutine(LoadAsyncScene(targetScene));
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        yield return new WaitForSeconds(3f); // tempo para exibir a imagem
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
