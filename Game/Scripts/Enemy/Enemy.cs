using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject player;
    [SerializeField] protected bool isAlive = true;
    protected Rigidbody2D rb;

}
