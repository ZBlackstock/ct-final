using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyConditional : Conditional
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected PlayerController playerController;

    public override void OnAwake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }
}
