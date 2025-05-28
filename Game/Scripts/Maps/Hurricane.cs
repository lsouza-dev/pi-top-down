using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurricane : MonoBehaviour
{
    public float speed = 3f;  // Velocidade de movimento do furacão
    public float minX;  // Limite mínimo do movimento no eixo X
    public float maxXf;  // Limite máximo do movimento no eixo X
    public float minY;  // Limite mínimo do movimento no eixo Y
    public float maxY;  // Limite máximo do movimento no eixo Y

    private float targetX;
    private float targetY;
    private bool movingRight = true;  // Direção inicial para a direita
    private bool movingUp = true;    // Direção inicial para cima

    void Start()
    {
        // Define a posição inicial do furacão
        targetX = Random.Range(minX, maxXf);
        targetY = Random.Range(minY, maxY);
    }

    void Update()
    {
        MoveHurricane();
    }

    void MoveHurricane()
    {
        // Move o furacão para a posição alvo no eixo X
        float currentX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        // Move o furacão para a posição alvo no eixo Y
        float currentY = Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime);

        // Atualiza a posição do furacão
        transform.position = new Vector2(currentX, currentY);

        // Quando atingir o limite (máximo ou mínimo) no eixo X, muda a direção
        if (currentX == targetX)
        {
            if (movingRight)
            {
                targetX = minX;  // Define o limite mínimo como alvo no eixo X
            }
            else
            {
                targetX = maxXf;  // Define o limite máximo como alvo no eixo X
            }
            movingRight = !movingRight;  // Inverte a direção no eixo X
        }

        // Quando atingir o limite (máximo ou mínimo) no eixo Y, muda a direção
        if (currentY == targetY)
        {
            if (movingUp)
            {
                targetY = minY;  // Define o limite mínimo como alvo no eixo Y
            }
            else
            {
                targetY = maxY;  // Define o limite máximo como alvo no eixo Y
            }
            movingUp = !movingUp;  // Inverte a direção no eixo Y
        }
    }
}