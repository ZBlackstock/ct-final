using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using UnityEngine;

// Enemy AI - spawns portal at position
public class BT_EnterPortal : EnemyAction
{
    public Transform portalSpawnPos;
    public float portalRot;
    public float portalDuration = 1;
    private PortalManager portalManager;
    private int Xscaler = 1;

    public override void OnAwake()
    {
        base.OnAwake();
        portalManager = GameObject.FindFirstObjectByType<PortalManager>();
    }

    public override void OnStart()
    {
        if (enemy.IsFaceRight() && Xscaler == -1 || !enemy.IsFaceRight() && Xscaler == 1)
        {
            Xscaler *= -1;
        }
        portalManager.SpawnPortal(new Vector2(portalSpawnPos.position.x, portalSpawnPos.position.y), Xscaler * portalRot, portalDuration);
        container.sounds.PlaySound(container.sounds.portalEnter);
    }
}
