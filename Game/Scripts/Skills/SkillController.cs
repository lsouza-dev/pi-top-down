using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillController : MonoBehaviour
{
    #region Variables

    [Header("Skill Settings")]
    [SerializeField] private Skill magePowerUp;
    [SerializeField] public int playerClass;

    [Header("Meteor")]
    [SerializeField] private GameObject meteorShadowPrefab;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject multidirectionalSpawner;
    [SerializeField] public Bullet currentBullet;

    private GameObject shadow;
    private Vector3 mousePosition;

    public bool isOnGround;
    public List<Transform> spawnPoints = new List<Transform>();

    public static SkillController instance;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        // Singleton
        instance = instance == null ? this : instance;

        // Load Resources
        magePowerUp = Resources.Load<Skill>("PowerUps\\Mage\\Meteor");
        meteorShadowPrefab = Resources.Load<GameObject>("PowerUps\\Shadow");

        // Get references
        rb = GetComponent<Rigidbody2D>();

        Transform spawnerParent = transform.Find("MultiDirectionalSpawners");
        if (spawnerParent != null)
        {
            spawnPoints = spawnerParent.GetComponentsInChildren<Transform>()
                                       .Where(t => t != spawnerParent)
                                       .ToList();
        }
        else
        {
            Debug.LogError("MultiDirectionalSpawners não encontrado como filho do Player.");
        }
    }

    private void Start()
    {
        currentBullet = ClassSelector.instance?.currentBullet;
    }

    private void Update()
    {
        mousePosition = GetMouseWorldPosition();
        mousePosition.z = 0;
    }

    #endregion

    #region Public Methods

    public void AttackOnMousePosition()
    {
        playerClass = ClassSelector.instance.currentClass;

        switch (playerClass)
        {
            case 2:
                CastMeteor();
                break;

            default:
                Debug.Log("Classe sem skill de meteoro.");
                break;
        }
    }

    #endregion

    #region Meteor Methods

    private void CastMeteor()
    {
        Vector3 spawnPosition = GetMeteorSpawnPosition();
        GameObject meteor = Instantiate(magePowerUp.gameObject, spawnPosition, Quaternion.identity);

        // Criar sombra no chão
        shadow = Instantiate(meteorShadowPrefab, mousePosition, Quaternion.identity);
        shadow.SetActive(true);
        StartCoroutine(AnimateShadowGrowth(shadow));

        // Esperar meteoro atingir o chão
        Rigidbody2D meteorRb = meteor.GetComponent<Rigidbody2D>();
        Skill skillScript = meteor.GetComponent<Skill>();
        StartCoroutine(WaitForMeteorToReachTarget(meteorRb, mousePosition.y, skillScript));
    }

    private IEnumerator WaitForMeteorToReachTarget(Rigidbody2D meteorRb, float targetY, Skill skillScript)
    {
        float timeout = 5f;
        float elapsed = 0f;

        while (meteorRb != null && meteorRb.transform.position.y > targetY && elapsed < timeout)
        {
            isOnGround = false;
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (meteorRb != null)
        {
            Destroy(shadow);
            isOnGround = true;
            meteorRb.gravityScale = 0;
            meteorRb.velocity = Vector2.zero;

            skillScript?.LaunchMeteorEffect();
        }
    }

    private IEnumerator AnimateShadowGrowth(GameObject shadow)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 1.5f;

        while (elapsed < duration && shadow != null)
        {
            float t = elapsed / duration;
            shadow.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            shadow.transform.localRotation = Quaternion.Euler(-72f, 0f, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (shadow != null)
        {
            shadow.transform.localScale = targetScale;
        }
    }

    private Vector3 GetMeteorSpawnPosition()
    {
        Camera cam = Camera.main;
        float cameraTopY = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane)).y;
        return new Vector3(mousePosition.x, cameraTopY + 1f, 0);
    }

    #endregion

    #region Utility

    private Vector3 GetMouseWorldPosition()
    {
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void MultiDirectionalAttack(int evolutionIndex)
    {
        Bullet bulletPrefab = currentBullet;

        // 1. Pega posição do mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // 2. Calcula a direção entre player e mouse
        Vector3 shootDirection = (mousePosition - PlayerController.instance.transform.position).normalized;

        // 3. Determina o spawner a ser usado (baseado na direção)
        Vector3 spawnerPos = GetSpawnerByDirection(shootDirection);

        // 4. Define número de projéteis com base na evolução
        int numberOfProjectiles = 3 + evolutionIndex; // Por exemplo: evolução 0 = 3 projéteis, etc.

        float spreadAngle = 30f; // Ângulo total de abertura do cone
        float angleStep = spreadAngle / (numberOfProjectiles - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float angleOffset = startAngle + i * angleStep;

            // Aplica rotação ao vetor de direção base
            Vector3 rotatedDirection = Quaternion.Euler(0, 0, angleOffset) * shootDirection;

            // Instancia projétil
            Bullet bullet = Instantiate(bulletPrefab, spawnerPos, Quaternion.identity);

            // Define rotação visual da bullet
            float bulletAngle = Mathf.Atan2(-rotatedDirection.y, -rotatedDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle);

            // Aplica velocidade
            bullet.GetComponent<Rigidbody2D>().velocity = rotatedDirection * bullet.speed;
        }
    }


    private Vector3 GetSpawnerByDirection(Vector3 direction)
    {
        var spawners = PlayerController.instance.GetComponent<PlayerAttack>().spawnersPositions;

        if (spawners == null || spawners.Count != 4)
        {
            Debug.LogWarning("Spawners não encontrados corretamente.");
            return PlayerController.instance.transform.position;
        }

        // Decide qual spawner usar baseado na direção
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Esquerda ou direita
            return direction.x > 0 ? spawners[1].transform.position : spawners[3].transform.position;
        }
        else
        {
            // Cima ou baixo
            return direction.y > 0 ? spawners[0].transform.position : spawners[2].transform.position;
        }
    }





    #endregion
}
