using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic health info to be used by entities
public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float health = 100;
    public uint healAmount = 4;
    protected bool hurt;
    [HideInInspector] public Rigidbody2D rb;

    public Animator tintAnim;
    public Animator bodyAnim;
    [HideInInspector] public Settings settings;

    public bool IsHurt()
    {
        return hurt;
    }
}
