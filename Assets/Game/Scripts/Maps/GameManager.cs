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
        Debug.Log("Todos os spawners foram destruídos!");
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
            Debug.Log($"Pergaminho '{scrollName}' coletado!");
        }
    }

    public bool HasScroll(string scrollName)
    {
        return scrollsCollected.Contains(scrollName);
    }

    public void SetBossScrollInteracted()
    {
        bossScrollInteracted = true;
        Debug.Log("Interação com o pergaminho do boss concluída!");
    }

    public bool HasInteractedWithBossScroll()
    {
        return bossScrollInteracted;
    }
}
