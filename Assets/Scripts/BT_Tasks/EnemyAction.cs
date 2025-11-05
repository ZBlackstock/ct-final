using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

// Enemy AI - Inherited by Enemy Actions
public class EnemyAction : Action
{
    protected Container container;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected PlayerController playerController;
    protected Enemy enemy;

    public override void OnAwake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponentInChildren<Animator>();
        playerController = PlayerController.FindFirstObjectByType<PlayerController>();
        enemy = Enemy.FindFirstObjectByType<Enemy>();
        container = Container.FindFirstObjectByType<Container>();
    }
}
