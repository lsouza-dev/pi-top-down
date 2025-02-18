using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform spawnerParent; // O spawner principal
    [SerializeField] private List<GameObject> spawnersPositions; // Lista dos spawners filhos
    [SerializeField] private List<GameObject> bullets;
    [SerializeField] private int currentBullet = 0;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float speed;

    // Posições relativas dos spawners filhos ao spawner principal
    private Vector3[] spawnerOffsets = new Vector3[]
    {
        new Vector3(0f, 0.3f, 0f),   // Posição 0 (Cima)
        new Vector3(0.6f, -0.2f, 0f), // Posição 1 (Direita)
        new Vector3(0f, -0.3f, 0f),  // Posição 2 (Baixo)
        new Vector3(-0.6f, -0.2f, 0f) // Posição 3 (Esquerda)
    };

    void Awake()
    {
        bullets = Resources.LoadAll<GameObject>("Bullets").ToList();
        spawnersPositions = Resources.LoadAll<GameObject>("Spawners").ToList();
        spawnerParent = GameObject.Find("Spawner").transform;
    }


    void Start()
    {
        // Certifique-se de que os spawners foram encontrados corretamente
        if (spawnerParent == null)
        {
            Debug.LogError("Spawner principal não está atribuído!");
            return;
        }
        
        if (spawnersPositions.Count != 4)
        {
            Debug.LogError("A lista de spawners filhos precisa ter exatamente 4 posições!");
            return;
        }

        PlayerController.instance.offsetTime = PlayerController.instance.offsetTimeDefault;
    }

    void Update()
    {
        // Atualiza a posição do spawner principal para seguir o jogador
        spawnerParent.position = PlayerController.instance.transform.position;

        // Atualiza a posição dos spawners filhos com base no pai
        for (int i = 0; i < spawnersPositions.Count; i++)
        {
            spawnersPositions[i].transform.position = spawnerParent.position + spawnerOffsets[i];
        }
    }

    public void Shoot()
    {
        if (spawnersPositions.Count == 0 || bullets.Count == 0) return;
        
        PlayerController.instance.isAttack = false;
        
        // Pegamos a posição do mouse na tela e convertemos para coordenadas do mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Pegamos a posição do spawner baseado na direção do mouse
        Transform spawner = spawnersPositions[PlayerController.instance.mouseDirection].transform;
        Vector3 spawnPosition = spawner.position;

        // Calculamos a direção do tiro
        Vector3 direction = (mousePosition - spawnPosition).normalized;

        // Criamos a bala
        GameObject bullet = Instantiate(bullets[currentBullet], spawnPosition, Quaternion.identity);
        PlayerController.instance.offsetTime = PlayerController.instance.offsetTimeDefault;
        

        // Calculamos o ângulo de rotação baseado no mouse
        float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Aplicamos a velocidade na direção calculada
        bullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
