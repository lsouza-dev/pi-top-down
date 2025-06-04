using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootController : MonoBehaviour
{
    private Animator anim;
    public MinionController minion;
    private MinionController instanceMinion;
    private float timeToRun = 2f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        minion = Resources.Load<MinionController>("Minions/EntMinion");
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeToRun -= Time.deltaTime;
        if (timeToRun <= 0)
        {
            Appear();
        }

    }

    public void Appear()
    {
        anim.SetTrigger("appearing");
        if (instanceMinion == null)
        {
            instanceMinion = Instantiate(minion, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
            instanceMinion.animator.SetTrigger("appear");
        }
    }

    public void Disappear()
    {
        anim.SetTrigger("disappearing");

    }

    public void DestroyRoot()
    {
        Destroy(gameObject);
    }
}
