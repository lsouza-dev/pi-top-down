using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private GameObject attributesUI;
    public string sceneName;
    public GameObject canvasPos;
    public GameObject attributesBackground;

    void Awake()
    {
        instance = instance == null ? this : instance;
        sceneName = SceneManager.GetActiveScene().name;
        attributesUI = Resources.Load<GameObject>("Attributes\\AttributeUpgradeController");
        canvasPos = GameObject.Find("Canvas");
        attributesBackground = GameObject.FindGameObjectWithTag("AttributesBackground");
        if(sceneName == "Forest") PlayerPrefs.SetInt("maxEnemiesOnSpawner", 1);
    }

    void Start()
    {
        Instantiate(attributesUI, canvasPos.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (attributesBackground.activeSelf)
            {
                attributesBackground.SetActive(false);
            }
            else
            {
                attributesBackground.SetActive(true);
            }
        }
    }
}
