using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpController : MonoBehaviour
{
    private Vector3 mousePosition;

    [SerializeField] private PowerUp magePowerUp;
    [SerializeField] public int playerClass;
    [SerializeField] private Rigidbody2D rb;
    public bool isOnGround;
    public static PowerUpController instance;
    public List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] public Bullet currentBullet;
    [SerializeField] public GameObject multidirectionalSpawner; // Prefab da bullet a ser instanciada

   void Awake()
{
    instance = instance == null ? this : instance;

    magePowerUp = Resources.Load<PowerUp>("PowerUps\\Mage\\Meteor");

    // Busca os spawnPoints dinamicamente a partir do MultiDirectionalSpawners
    Transform spawnerParent = transform.Find("MultiDirectionalSpawners");
    if (spawnerParent != null)
        spawnPoints = spawnerParent.GetComponentsInChildren<Transform>().Where(t => t != spawnerParent).ToList();
    else
        Debug.LogError("MultiDirectionalSpawners não encontrado como filho do Player.");
    

    rb = GetComponent<Rigidbody2D>();
}

    void Start()
    {
        currentBullet = ClassSelector.instance.currentBullet;
    }

    void Update()
    {
        // Captura a posição do mouse com base na Cinemachine Virtual Camera
        mousePosition = GetMouseWorldPosition();
        mousePosition.z = 0;

    }

    public void AttackOnMousePosition()
    {
        playerClass = ClassSelector.instance.currentClass;
        print("Player Class on PowerUpController: " + playerClass);

        switch (playerClass)
        {
            case 0:
                print("Arqueiro");
                break;
            case 1:
                print("Guerreiro");
                break;
            case 2:
                Vector3 spawnPosition = GetSpawnPosition();
                // Instancia o meteoro na posição correta
                GameObject meteor = Instantiate(magePowerUp.gameObject, spawnPosition, Quaternion.identity);

                // Inicia a verificação para zerar a massa quando atingir o destino
                StartCoroutine(WaitForMeteorToReachTarget(meteor.GetComponent<Rigidbody2D>(), mousePosition.y));
                break;
            default:
                print("Classe inválida para utilizar Skill");
                break;
        }
    }

    public void MultiDirectionalAttack(int playerClass)
    {
        print("Player Class on PowerUpController: " + playerClass);
        switch (playerClass)
        {
            case 0:
                SpawnBullets(playerClass);
                break;
            case 1:
                SpawnBullets(playerClass);
                break;
            case 2:
                SpawnBullets(playerClass);
                break;
            default:
                print("Classe inválida para utilizar Skill");
                break;
        }
    }

    private void SpawnBullets(int playerClass)
    {
        if (spawnPoints.Count == 0) return;
        print("SpawnPoints Count: " + spawnPoints.Count);
        switch (playerClass)
        {
            case 0:
                SpawnBulletsInDirections(1); // Exemplo: 3 bullets (uma reta e duas diagonais)
                break;
            case 1: 
                SpawnBulletsInDirections(3); // Exemplo: 5 bullets (mais ângulos)
                break;
            case 2: 
                SpawnBulletsInDirections(5); // Exemplo: 7 bullets (ainda mais ângulos)
                break;
            default:
                print("Classe inválida para utilizar Skill");
                break;
        }
    }

    private void SpawnBulletsInDirections(int bulletCount)
    {
        if (bulletCount < 1 || bulletCount > 5) return; // Garante que o número de bullets seja válido (1 a 3)

        // Define os índices dos spawners com base no bulletCount
        List<int> spawnerIndices = new List<int>();
        if (bulletCount == 1)
        {
            spawnerIndices.Add(2); // Apenas o spawner central
        }
        else if (bulletCount == 3)
        {
            spawnerIndices.AddRange(new[] { 1, 2, 3 }); // Spawners 1, 2 e 3
        }
        else if (bulletCount == 5)
        {
            spawnerIndices.AddRange(new[] { 0, 1, 2, 3, 4 }); // Todos os spawners
        }

        // Calcula o ângulo inicial e o incremento entre as bullets
        float angleStep = spawnerIndices.Count > 1 ? 45f / (spawnerIndices.Count - 1) : 0f; // Evita divisão por zero
        float startAngle = -22.5f; // Começa com um ângulo inclinado para cima

        for (int i = 0; i < spawnerIndices.Count; i++)
        {
            // Calcula o ângulo para cada bullet
            float angle = startAngle + (i * angleStep);

            // Converte o ângulo para um vetor de direção
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            // Garante que o vetor de direção seja válido
            if (direction.magnitude == 0)
            {
                Debug.LogWarning($"Direção inválida para a bullet no índice {i}. Ignorando esta instância.");
                continue;
            }

            direction.Normalize(); // Normaliza o vetor de direção

            // Obtém a posição do spawner correspondente
            if (spawnerIndices[i] < 0 || spawnerIndices[i] >= spawnPoints.Count)
            {
                Debug.LogWarning($"Spawner inválido no índice {i}. Ignorando esta instância.");
                continue;
            }

            Transform spawner = spawnPoints[spawnerIndices[i]];
            Vector3 spawnPosition = spawner.position;

            // Instancia a bullet
            var bulletInstance = Instantiate(currentBullet, spawnPosition, Quaternion.identity);

            // Define a rotação da bullet
            bulletInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Define a velocidade da bullet
            bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * currentBullet.speed;
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // Obtém a posição do topo da câmera virtual
        Camera cam = Camera.main;
        float cameraTopY = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane)).y;

        // Define a posição inicial do meteoro (X do mouse, Y acima da câmera)
        return new Vector3(mousePosition.x, cameraTopY + 1f, 0);
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Obtém a posição do mouse convertida para o mundo
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private IEnumerator WaitForMeteorToReachTarget(Rigidbody2D meteorRb, float targetY)
    {
        while (meteorRb.transform.position.y > targetY)
        {
            isOnGround = false;
            yield return null; // Espera um frame antes de verificar novamente
        }

        // Quando o meteoro atingir o Y do mouse, zera a gravidade e a velocidade
        isOnGround = true;
        meteorRb.gravityScale = 0;
        meteorRb.velocity = Vector2.zero;
    }
}
