using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public (SpriteLibraryAsset, Bullet) ClassChoice(int classIndex)
    {
        this.evolutionIndex += 1;
        this.currentClass = classIndex;

        print($"Evolution: {currentEvolution}, Class: {currentClass}");
        if (evolutionIndex <= playerEvolutions.Count)
        {
            switch (evolutionIndex)
            {
                case 0:

                    switch (currentClass)
                    {
                        case 0:
                            currentLibrary = goblinLibraries[currentClass];
                            currentBullet = bullets[currentClass];
                            break;
                        case 1:
                            currentLibrary = goblinLibraries[currentClass];
                            currentBullet = bullets[currentClass];
                            break;
                        case 2:
                            currentLibrary = goblinLibraries[currentClass];
                            currentBullet = bullets[currentClass];
                            break;

                    }
                    break;
                case 1:

                    switch (currentClass)
                    {
                        case 0:

                            currentLibrary = orclinLibraries[currentClass];

                            currentBullet = bullets[currentClass];

                            break;
                        case 1:
                            currentLibrary = orclinLibraries[currentClass];
                            currentBullet = bullets[currentClass];
                            break;
                        case 2:
                            currentLibrary = orclinLibraries[currentClass];
                            currentBullet = bullets[currentClass];
                            break;

                    }
                    break;
                case 2:

                    switch (currentClass)
                    {
                        case 0:
                            currentLibrary = orcLibraries[currentClass];
                            currentBullet = bullets[currentClass];
                            break;
                        case 1:
                            currentLibrary = orcLibraries[currentClass];
                            currentBullet = bullets[currentClass];
                            break;
                        case 2:
                            currentLibrary = orcLibraries[currentClass];
                            currentBullet = bullets[currentClass];
                            break;
                    }
                    break;
            }
        }
        print($"{currentLibrary.name} - {currentBullet}");
        return (currentLibrary, currentBullet);

    }

}
