using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class ClassSelector : MonoBehaviour
{

    [Header("Libraries")]
    [SerializeField] public List<SpriteLibraryAsset> goblinLibraries;
    [SerializeField] public List<SpriteLibraryAsset> orclinLibraries;
    [SerializeField] public List<SpriteLibraryAsset> orcLibraries;
    [SerializeField] public SpriteLibraryAsset currentLibrary;

    [Header("Bullets")]
    [SerializeField] public List<Bullet> bullets;
    [SerializeField] public Bullet currentBullet;

    [SerializeField] public PlayerController playerController;

    [Header("Player Mode")]
    [SerializeField] public List<int> playerEvolutions = new List<int>() { 0, 1, 2 };
    [SerializeField] public int currentEvolution;
    [SerializeField] public List<int> playerClass = new List<int>() { 0, 1, 2 };
    [SerializeField] public int currentClass;
    [SerializeField] public Vector2 colliderOffset;
    [SerializeField] public Vector2 colliderSize;
    [SerializeField] public Vector3[] spawnersOffset;



    public static ClassSelector instance;
    public int evolutionIndex = -1;


    void Awake()
    {
        bullets = Resources.LoadAll<Bullet>("Bullets").ToList();

        goblinLibraries = Resources.LoadAll<SpriteLibraryAsset>("Libraries\\Goblin").ToList();
        orclinLibraries = Resources.LoadAll<SpriteLibraryAsset>("Libraries\\Orclin").ToList();
        orcLibraries = Resources.LoadAll<SpriteLibraryAsset>("Libraries\\Orc").ToList();

        playerController = FindObjectOfType<PlayerController>();

        instance = instance == null ? instance = this : instance;
    }

    public (SpriteLibraryAsset, Bullet, Vector2, Vector2, Vector3[]) ClassChoice(int classIndex)
    {
        this.evolutionIndex += 1;
        this.currentClass = classIndex;


        print($"Evolution: {currentEvolution}, Class: {currentClass}");
        if (evolutionIndex <= playerEvolutions.Count)
        {
            switch (evolutionIndex)
            {
                case 0:
                    colliderOffset = new Vector2(0.0175f, -0.176f);
                    colliderSize = new Vector2(0.615f, 0.752f);
                    switch (currentClass)
                    {
                        case 0:
                            currentLibrary = goblinLibraries[currentClass];
                            currentBullet = bullets[currentClass];

                            spawnersOffset = new Vector3[]{
                                new Vector3(0f, 0.3f, 0f),   // Posição 0 (Cima)
                                new Vector3(0.6f, -0.2f, 0f), // Posição 1 (Direita)
                                new Vector3(0f, -0.6f, 0f),  // Posição 2 (Baixo)
                                new Vector3(-0.6f, -0.2f, 0f) // Posição 3 (Esquerda)
                            };

                            break;
                        case 1:
                            currentLibrary = goblinLibraries[currentClass];
                            currentBullet = bullets[currentClass];

                            spawnersOffset = new Vector3[]{
                                new Vector3(0f, 0.3f, 0f),   // Posição 0 (Cima)
                                new Vector3(0.6f, -0.2f, 0f), // Posição 1 (Direita)
                                new Vector3(0f, -0.6f, 0f),  // Posição 2 (Baixo)
                                new Vector3(-0.6f, -0.2f, 0f) // Posição 3 (Esquerda)
                            };

                            break;
                        case 2:
                            currentLibrary = goblinLibraries[currentClass];
                            currentBullet = bullets[currentClass];

                            spawnersOffset = new Vector3[]{
                                new Vector3(-.06f, 0.3f, 0f),   // Posição 0 (Cima)
                                new Vector3(0.7f, -0.25f, 0f), // Posição 1 (Direita)
                                new Vector3(-.06f, -0.6f, 0f),  // Posição 2 (Baixo)
                                new Vector3(-0.7f, -0.25f, 0f) // Posição 3 (Esquerda)
                            };
                            break;

                    }
                    break;
                case 1:

                    switch (currentClass)
                    {
                        case 0:

                            currentLibrary = orclinLibraries[currentClass];
                            currentBullet = bullets[currentClass];


                            colliderOffset = new Vector2(0.0262f, -0.176f);
                            colliderSize = new Vector2(0.560f, 0.752f);

                            spawnersOffset = new Vector3[]{
                                new Vector3(0f, 0.3f, 0f),   // Posição 0 (Cima)
                                new Vector3(0.6f, -0.2f, 0f), // Posição 1 (Direita)
                                new Vector3(0f, -0.6f, 0f),  // Posição 2 (Baixo)
                                new Vector3(-0.6f, -0.2f, 0f) // Posição 3 (Esquerda)
                            };

                            break;
                        case 1:
                            currentLibrary = orclinLibraries[currentClass];
                            currentBullet = bullets[currentClass];

                            colliderOffset = new Vector2(0.206f, -0.163f);
                            colliderSize = new Vector2(0.633f, 0.7627f);
                            break;
                        case 2:
                            currentLibrary = orclinLibraries[currentClass];
                            currentBullet = bullets[currentClass];

                            colliderOffset = new Vector2(0.09015f, -0.163f);
                            colliderSize = new Vector2(0.6773f, 0.7627f);
                            break;

                    }
                    break;
                case 2:

                    switch (currentClass)
                    {
                        case 0:
                            currentLibrary = orcLibraries[currentClass];
                            currentBullet = bullets[currentClass];

                            colliderOffset = new Vector2(0.0175f, -0.00882f);
                            colliderSize = new Vector2(0.7218f, 0.7218f);

                            spawnersOffset = new Vector3[]{
                                new Vector3(0.5f, 0.42f, 0f),   // Posição 0 (Cima)
                                new Vector3(0.6f, -0.1f, 0f), // Posição 1 (Direita)
                                new Vector3(0.05f, -0.6f, 0f),  // Posição 2 (Baixo)
                                new Vector3(-0.6f, -0.1f, 0f) // Posição 3 (Esquerda)
                            };


                            break;
                        case 1:
                            currentLibrary = orcLibraries[currentClass];
                            currentBullet = bullets[currentClass];

                            colliderOffset = new Vector2(0.4309f, -0.1919f);
                            colliderSize = new Vector2(0.6773f, 0.697f);

                            spawnersOffset = new Vector3[]{
                                new Vector3(0.42f, 0.42f, 0f),   // Posição 0 (Cima)
                                new Vector3(0.8f, -0.27f, 0f), // Posição 1 (Direita)
                                new Vector3(0.42f, -0.6f, 0f),  // Posição 2 (Baixo)
                                new Vector3(-0.8f, -0.27f, 0f), // Posição 3 (Esquerda)
                            };


                            break;
                        case 2:
                            currentLibrary = orcLibraries[currentClass];
                            currentBullet = bullets[currentClass];


                            colliderOffset = new Vector2(0.2422f, -0.1945f);
                            colliderSize = new Vector2(0.6880f, 0.7632f);

                            spawnersOffset = new Vector3[]{
                                new Vector3(0.23f, 0.42f, 0f),   // Posição 0 (Cima)
                                new Vector3(0.8f, -0.27f, 0f), // Posição 1 (Direita)
                                new Vector3(0.23f, -0.6f, 0f),  // Posição 2 (Baixo)
                                new Vector3(-0.8f, -0.27f, 0f), // Posição 3 (Esquerda)
                            };
                            break;
                    }
                    break;
            }
        }
        print($"{currentLibrary.name} - {currentBullet}");

        for(int i = 0; i < spawnersOffset.Length; i++) print(spawnersOffset[i]);

        return (currentLibrary, currentBullet, colliderOffset, colliderSize, spawnersOffset);

    }
}
