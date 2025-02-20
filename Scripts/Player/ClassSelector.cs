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
    [SerializeField] public List<Bullet> goblinBullets;
    [SerializeField] public List<Bullet> orclinBullets;
    [SerializeField] public List<Bullet> orcBullets;
    [SerializeField] public Bullet currentBullet;

    [SerializeField] public PlayerController playerController;

    [Header("Player Mode")]
    [SerializeField] public List<int> playerEvolutions = new List<int>() { 0, 1, 2 };
    [SerializeField] public int currentEvolution;
    [SerializeField] public List<int> playerClass = new List<int>() { 0, 1, 2 };
    [SerializeField] public int currentClass;


    public static ClassSelector instance;


    void Awake()
    {
        goblinBullets = Resources.LoadAll<Bullet>("Bullets\\Goblin").ToList();
        goblinLibraries = Resources.LoadAll<SpriteLibraryAsset>("Libraries\\Goblin").ToList();

        orclinBullets = Resources.LoadAll<Bullet>("Bullets\\Orclin").ToList();
        orclinLibraries = Resources.LoadAll<SpriteLibraryAsset>("Libraries\\Orclin").ToList();

        orcBullets = Resources.LoadAll<Bullet>("Bullets\\Orc").ToList();
        orcLibraries = Resources.LoadAll<SpriteLibraryAsset>("Libraries\\Orc").ToList();

        playerController = FindObjectOfType<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = instance == null ? instance = this : instance;
    }


    public (SpriteLibraryAsset, Bullet) ClassChoice(int evolution, int classIndex)
    {
        print($"Evolution: {evolution}, Class: {classIndex}");
        currentEvolution = playerEvolutions[evolution];
        currentClass = playerClass[classIndex];

        switch (currentEvolution)
        {
            case 0:
                switch (currentClass)
                {
                    case 0:
                        currentLibrary = goblinLibraries[classIndex];
                        currentBullet = goblinBullets[classIndex];
                    break;
                    case 1:
                        currentLibrary = goblinLibraries[classIndex];
                        currentBullet = goblinBullets[classIndex];
                    break;
                    case 2:
                        currentLibrary = goblinLibraries[classIndex];
                        currentBullet = goblinBullets[classIndex];
                    break;

                }
            break;
            case 1:
                switch (currentClass)
                {
                    case 0:
                        currentLibrary = orclinLibraries[classIndex];
                        currentBullet = orclinBullets[classIndex];
                    break;
                    case 1:
                        currentLibrary = orclinLibraries[classIndex];
                        currentBullet = orclinBullets[classIndex];
                    break;
                    case 2:
                        currentLibrary = orclinLibraries[classIndex];
                        currentBullet = orclinBullets[classIndex];
                    break;

                }
            break;
            case 2:
                switch (currentClass)
                {
                    case 0:
                        currentLibrary = orcLibraries[classIndex];
                        currentBullet = orcBullets[classIndex];
                    break;
                    case 1:
                        currentLibrary = orcLibraries[classIndex];
                        currentBullet = orcBullets[classIndex];
                    break;
                    case 2:
                        currentLibrary = orcLibraries[classIndex];
                        currentBullet = orcBullets[classIndex];
                    break;
                }
            break;
            default:
                return (null, null);
        }

        return (currentLibrary, currentBullet);

    }

}
