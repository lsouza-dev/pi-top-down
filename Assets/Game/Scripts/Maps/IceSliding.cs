using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSliding : MonoBehaviour
{
    public float slideSpeed = 5f; // Velocidade ao deslizar
    public float gridSize = 0.5f;  // Tamanho do grid (ex: 1 unidade)

    private Vector2 slideDirection;
    private bool isSliding = false;
    private bool onIce = false;
    private Rigidbody2D rb;
    private PlayerController playerMovement;

    void Start()
    {
        string scene = PlayerPrefs.GetString("TargetScene");
        if (!scene.Equals("Snow")) this.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!isSliding && playerMovement.enabled && onIce)
        {
            Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            if (inputDirection != Vector2.zero)
            {
                SnapToGrid();
                StartSliding(inputDirection);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ice"))
        {
            onIce = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ice"))
        {
            onIce = false;
        }
    }

    private void StartSliding(Vector2 inputDirection)
    {
        slideDirection = GetOrthogonalDirection(inputDirection);

        if (slideDirection != Vector2.zero)
        {
            isSliding = true;
            playerMovement.enabled = false;
            rb.velocity = slideDirection * slideSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (isSliding)
        {
            rb.velocity = slideDirection * slideSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSliding)
        {
            StopSliding();
        }
    }

    private void StopSliding()
    {
        isSliding = false;
        rb.velocity = Vector2.zero;
        SnapToGrid(); 

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }

    private Vector2 GetOrthogonalDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return new Vector2(Mathf.Sign(direction.x), 0);
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            return new Vector2(0, Mathf.Sign(direction.y));
        }
        return Vector2.zero;
    }

    private void SnapToGrid()
    {
        Vector2 position = rb.position;
        position.x = Mathf.Round(position.x / gridSize) * gridSize + 0.2f;
        position.y = Mathf.Round(position.y / gridSize) * gridSize - 0.25f;
        rb.MovePosition(position); // Usa MovePosition para evitar conflitos com a f�sica
    }
}