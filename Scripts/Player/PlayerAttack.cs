using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform spawnerParent; // O spawner principal
    [SerializeField] private List<GameObject> spawnersPositions; // Lista dos spawners filhos
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float speed;

    // Posições relativas dos spawners filhos ao spawner principal
    public Vector3[] spawnerOffsets;

    void Awake()
    {
        spawnersPositions = Resources.LoadAll<GameObject>("Spawners").ToList();
        spawnerParent = GameObject.Find("Spawner").transform;
    }

    void Start()
    {
        // Certifique-se de que os spawners foram encontrados corretamente
        if (spawnerParent == null)
        {
            Debug.LogError("Spawner principal não está atribuído!");
            return;
        }
        
        if (spawnersPositions.Count != 4)
        {
            Debug.LogError("A lista de spawners filhos precisa ter exatamente 4 posições!");
            return;
        }
    }

    void Update()
    {
        // Atualiza a posição do spawner principal para seguir o jogador
        spawnerParent.position = PlayerController.instance.transform.position;

        // Atualiza a posição dos spawners filhos com base no pai
        for (int i = 0; i < spawnersPositions.Count; i++)
        {
            spawnersPositions[i].transform.position = spawnerParent.position + spawnerOffsets[i];
        }
    }

    public void Shoot(Bullet bullet)
    {
        if (spawnersPositions.Count == 0 ) return;
        
        PlayerController.instance.isAttack = false;
        
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        
        Transform spawner = spawnersPositions[PlayerController.instance.mouseDirection].transform;
        Vector3 spawnPosition = spawner.position;

        
        Vector3 direction = (mousePosition - spawnPosition).normalized;

        
        var bulletInstance = Instantiate(bullet, spawnPosition, Quaternion.identity);
        PlayerController.instance.shootCooldownTime = PlayerController.instance.shootCooldownTimeDefault;
        

        
        float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        bulletInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        
        bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
}
