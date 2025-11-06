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
    public bool plungeAttack;

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
        if (plungeAttack)
        {
            trans.position = teleportTrans.position; // Use X and Y
        }
        else
        {
            trans.position = new Vector2(teleportTrans.position.x, 8); // Just Use X
        }

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
