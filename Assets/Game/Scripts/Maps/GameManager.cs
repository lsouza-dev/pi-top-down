using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool allSpawnersDestroyed = false;
    private HashSet<string> scrollsCollected = new HashSet<string>();
    private bool bossScrollInteracted = false;

    private void Awake()
    {
        Instance = Instance == null ? this : Instance;
    }

    public void SetAllSpawnersDestroyed()
    {
        allSpawnersDestroyed = true;
    }

    public bool AreAllSpawnersDestroyed()
    {
        return allSpawnersDestroyed;
    }

    public void CollectScroll(string scrollName)
    {
        if (!scrollsCollected.Contains(scrollName))
        {
            scrollsCollected.Add(scrollName);
        }
    }

    public bool HasScroll(string scrollName)
    {
        return scrollsCollected.Contains(scrollName);
    }

    public void SetBossScrollInteracted()
    {
        bossScrollInteracted = true;
    }

    public bool HasInteractedWithBossScroll()
    {
        return bossScrollInteracted;
    }
}
