using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public int maxEnemiesOnSpawner;
    public string sceneName;

    void Awake()
    {
    }


    void Start()
    {
        sceneName = GameController.instance.sceneName;
        if (sceneName != null)
        {
            GetMaxEnemiesSpawn(sceneName);
            SpawnEnemies();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GetMaxEnemiesSpawn(string sceneName)
    {
        switch (sceneName)
        {
            case "Forest":
                enemies = Resources.LoadAll<Enemy>("Enemy\\Forest").ToList();
                break;
            case "Desert":
                enemies = Resources.LoadAll<Enemy>("Enemy\\Desert").ToList();
                break;
            case "Snow":
                enemies = Resources.LoadAll<Enemy>("Enemy\\Snow").ToList();
                break;
            case "Swamp":
                enemies = Resources.LoadAll<Enemy>("Enemy\\Swamp").ToList();
                break;
            default:
                enemies = null;
                break;
        }

        maxEnemiesOnSpawner = PlayerPrefs.GetInt("maxEnemiesOnSpawner") + 1;
        PlayerPrefs.SetInt("maxEnemiesOnSpawner", maxEnemiesOnSpawner);
    }

    private void SpawnEnemies()
    {
        for (int i = 1; i <= maxEnemiesOnSpawner; i++)
        {
            int randomEnemy = Random.Range(0, enemies.Count);
            var enemy = enemies[randomEnemy];
            Instantiate(enemy, new Vector3(Random.Range(transform.position.x - 1, transform.position.x + 1), Random.Range(transform.position.y - 1, transform.position.y + 1), 0), Quaternion.identity);
        }
    }
}
