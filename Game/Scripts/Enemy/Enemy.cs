using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health = 5f;
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject player;
    [SerializeField] protected bool isAlive = true;
    protected Rigidbody2D rb;
    // Start is called before the first frame update
}
