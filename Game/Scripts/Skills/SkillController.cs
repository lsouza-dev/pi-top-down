using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillController : MonoBehaviour
{
    private Vector3 mousePosition;

    [SerializeField] private Skill magePowerUp;
    [SerializeField] public int playerClass;
    [SerializeField] private Rigidbody2D rb;
    public bool isOnGround;
    public static SkillController instance;
    public List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] public Bullet currentBullet;
    [SerializeField] public GameObject multidirectionalSpawner; // Prefab da bullet a ser instanciada
    public GameObject meteorShadowPrefab; // Referência pública ao prefab da sombra
    private GameObject shadow;

    void Awake()
    {
        instance = instance == null ? this : instance;

        magePowerUp = Resources.Load<Skill>("PowerUps\\Mage\\Meteor");
        meteorShadowPrefab = Resources.Load<GameObject>("PowerUps\\Shadow");

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

                // Instancia o meteoro
                GameObject meteor = Instantiate(magePowerUp.gameObject, spawnPosition, Quaternion.identity);

                // Instancia a sombra no chão, no ponto do mouse
                shadow = Instantiate(meteorShadowPrefab, mousePosition, Quaternion.identity);
                shadow.SetActive(true);
                StartCoroutine(AnimateShadowGrowth(shadow));

                // Verifica quando o meteoro toca o chão
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
                SpawnBulletsInDirections(playerClass); // Exemplo: 3 bullets (uma reta e duas diagonais)
                break;
            case 1:
                SpawnBulletsInDirections(playerClass); // Exemplo: 5 bullets (mais ângulos)
                break;
            case 2:
                SpawnBulletsInDirections(playerClass); // Exemplo: 7 bullets (ainda mais ângulos)
                break;
            default:
                print("Classe inválida para utilizar Skill");
                break;
        }
    }

    private void SpawnBulletsInDirections(int evolutionLevel)
    {
        if (evolutionLevel < 0 || evolutionLevel > 2) return;

        // Define os spawners com base na evolução
        Dictionary<int, List<int>> evolutionSpawns = new Dictionary<int, List<int>>
    {
        { 0, new List<int> { 2 } },
        { 1, new List<int> { 1, 2, 3 } },
        { 2, new List<int> { 0, 1, 2, 3, 4 } }
    };

        List<int> spawnerIndices = evolutionSpawns[evolutionLevel];
        float directionMultiplier = transform.localScale.x > 0 ? -1f : 1f;

        foreach (int index in spawnerIndices)
        {
            if (index < 0 || index >= spawnPoints.Count)
            {
                Debug.LogWarning($"Spawner inválido no índice {index}. Ignorando.");
                continue;
            }

            Transform spawner = spawnPoints[index];
            Vector3 spawnPosition = spawner.position;
            float angle = GetBulletAngle(index) * directionMultiplier;

            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized * directionMultiplier;

            var bulletInstance = Instantiate(currentBullet, spawnPosition, Quaternion.Euler(0, 0, angle));
            bulletInstance.damage *= 1.25f;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * currentBullet.speed;
        }
    }

    private float GetBulletAngle(int index)
    {
        switch (index)
        {
            case 0: return 45f;  // Diagonal para cima à esquerda
            case 1: return 22.5f; // Levemente inclinado para cima
            case 2: return 0f;   // Reto
            case 3: return -22.5f; // Levemente inclinado para baixo
            case 4: return -45f; // Diagonal para baixo à direita
            default: return 0f;
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
        Destroy(shadow);
        isOnGround = true;
        meteorRb.gravityScale = 0;
        meteorRb.velocity = Vector2.zero;
    }


    private IEnumerator AnimateShadowGrowth(GameObject shadow)
    {
        float duration = 1f; // Mesmo tempo de queda do meteoro
        float elapsed = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 1.5f;

        while (elapsed < duration)
        {
            if (shadow != null)
            {
                float t = elapsed / duration;
                shadow.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                shadow.transform.localRotation = new Quaternion(-72f, 0f, 0f, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

}
