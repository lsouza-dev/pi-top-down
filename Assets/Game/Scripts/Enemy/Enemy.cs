using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public Animator animator;
    [SerializeField] protected GameObject player;
    [SerializeField] public bool isAlive = true;
    public Rigidbody2D rb;

}
