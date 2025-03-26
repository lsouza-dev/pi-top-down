using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpController : MonoBehaviour
{
    private Vector3 mousePosition;

    [SerializeField] private PowerUp magePowerUp;
    [SerializeField] private int playerClass;
    [SerializeField] private Rigidbody2D rb;
    public bool isOnGround;
    public static PowerUpController instance;

    void Awake()
    {
        instance = instance == null ? instance = this : instance;

        magePowerUp = Resources.Load<PowerUp>("PowerUps\\Mage\\Meteor");
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerClass = ClassSelector.instance.currentClass;
    }

    void Update()
    {
        // Captura a posição do mouse com base na Cinemachine Virtual Camera
        mousePosition = GetMouseWorldPosition();
        mousePosition.z = 0;

        // Quando o meteoro atingir o mouse, zera a gravidade
        if (transform.position.y <= mousePosition.y)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }
    }

    public void AttackOnMousePosition()
    {
        print(mousePosition);

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

    public void MultiDirectionalAttack() { }

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
            yield return null; // Espera um frame antes de verificar novamente
        }

        // Quando o meteoro atingir o Y do mouse, zera a gravidade e a velocidade
        isOnGround = true;
        meteorRb.gravityScale = 0;
        meteorRb.velocity = Vector2.zero;
    }
}
