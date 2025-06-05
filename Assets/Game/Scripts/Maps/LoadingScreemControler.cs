using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public Image loadingImage;
    public Sprite forestSprite;
    public Sprite swampSprite;
    public Sprite snowSprite;
    public Sprite desertSprite;
    public Sprite finalBossSprite;

    void Awake()
    {
        loadingImage = GetComponent<Image>();
    }
    void Start()
    {
        string targetScene = PlayerPrefs.GetString("TargetScene");
        string loadingKey = "Loading_" + targetScene;
        print(targetScene);

        switch (targetScene)
        {
            case "Forest":
                loadingImage.sprite = forestSprite;
                break;
            case "Swamp":
                loadingImage.sprite = swampSprite;
                break;
            case "Snow":
                loadingImage.sprite = snowSprite;
                break;
            case "Desert":
                loadingImage.sprite = desertSprite;                
                transform.localScale = new Vector3(-1, 1, 1);
                break;
            case "FinalBoss":
                loadingImage.sprite = finalBossSprite;
                break;
            default:
                print("cena nao encontrada");
                break;
        }
        
        StartCoroutine(LoadAsyncScene(targetScene));
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        yield return new WaitForSeconds(7f); // tempo para exibir a imagem
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
