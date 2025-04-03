using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Captura entrada do teclado ou joystick
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize(); // Evita movimento mais rápido na diagonal
    }

    void FixedUpdate()
    {
        // Move o personagem
        rb.velocity = movement * speed;
    }
}
