using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100;
    public float health = 100;
    public uint healAmount = 4;

    [HideInInspector] public Rigidbody2D rb;

    public Animator tintAnim;
    public Animator bodyAnim;
    [HideInInspector] public Settings settings;

}
