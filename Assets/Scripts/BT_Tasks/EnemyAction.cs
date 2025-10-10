using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class EnemyAction : Action
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected PlayerController playerController;

    public override void OnAwake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
        playerController = PlayerController.FindFirstObjectByType<PlayerController>();
    }
}
