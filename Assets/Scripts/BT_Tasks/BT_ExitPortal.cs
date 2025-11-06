using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using UnityEngine;

// Enemy AI - spawns portal and moves enemy position for teleporting attacks
public class BT_ExitPortal : EnemyAction
{
    private Transform trans;
    public Transform teleportTrans;

    [Header("Portal")]
    public Vector2 portalOffset;
    public float portalRot;
    public float portalDuration;
    private PortalManager portalManager;
    private int Xscaler = 1;

    public override void OnAwake()
    {
        base.OnAwake();
        trans = enemy.transform;
        portalManager = GameObject.FindFirstObjectByType<PortalManager>();
    }

    public override void OnStart()
    {
        // Teleport Enemy
        // Change this to detect if it's plunging attack
        trans.position = new Vector2(teleportTrans.position.x, 8);

        enemy.FlipCheck(false);
        if (enemy.IsFaceRight() && Xscaler == -1 || !enemy.IsFaceRight() && Xscaler == 1)
        {
            Xscaler *= -1;
        }

        // Spawn portal based on enemy pos
        portalManager.SpawnPortal(new Vector2(trans.position.x, trans.position.y) +
                                  new Vector2(portalOffset.x * Xscaler, portalOffset.y),
                                  portalRot * Xscaler, portalDuration);
    }
}
