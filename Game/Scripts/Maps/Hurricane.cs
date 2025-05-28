using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurricane : MonoBehaviour
{
    public float speed = 3f;  // Velocidade de movimento do furac�o
    public float minX;  // Limite m�nimo do movimento no eixo X
    public float maxXf;  // Limite m�ximo do movimento no eixo X
    public float minY;  // Limite m�nimo do movimento no eixo Y
    public float maxY;  // Limite m�ximo do movimento no eixo Y

    private float targetX;
    private float targetY;
    private bool movingRight = true;  // Dire��o inicial para a direita
    private bool movingUp = true;    // Dire��o inicial para cima

    void Start()
    {
        // Define a posi��o inicial do furac�o
        targetX = Random.Range(minX, maxXf);
        targetY = Random.Range(minY, maxY);
    }

    void Update()
    {
        MoveHurricane();
    }

    void MoveHurricane()
    {
        // Move o furac�o para a posi��o alvo no eixo X
        float currentX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        // Move o furac�o para a posi��o alvo no eixo Y
        float currentY = Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime);

        // Atualiza a posi��o do furac�o
        transform.position = new Vector2(currentX, currentY);

        // Quando atingir o limite (m�ximo ou m�nimo) no eixo X, muda a dire��o
        if (currentX == targetX)
        {
            if (movingRight)
            {
                targetX = minX;  // Define o limite m�nimo como alvo no eixo X
            }
            else
            {
                targetX = maxXf;  // Define o limite m�ximo como alvo no eixo X
            }
            movingRight = !movingRight;  // Inverte a dire��o no eixo X
        }

        // Quando atingir o limite (m�ximo ou m�nimo) no eixo Y, muda a dire��o
        if (currentY == targetY)
        {
            if (movingUp)
            {
                targetY = minY;  // Define o limite m�nimo como alvo no eixo Y
            }
            else
            {
                targetY = maxY;  // Define o limite m�ximo como alvo no eixo Y
            }
            movingUp = !movingUp;  // Inverte a dire��o no eixo Y
        }
    }
}