using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using UnityEngine;

// Enemy AI - spawns portal and moves enemy position for teleporting attacks
public class BT_SpawnPortal : EnemyAction
{
    private Transform trans;
    public Transform teleportTrans;

    [Header("Portal")]
    public Vector2 portalOffset;
    public float portalRot;
    public float portalDuration;
    private PortalManager portalManager;

    public override void OnAwake()
    {
        base.OnAwake();
        trans = enemy.transform;
        portalManager = GameObject.FindFirstObjectByType<PortalManager>();
    }

    public override void OnStart()
    {
        trans.position = teleportTrans.position;
        portalManager.SpawnPortal(new Vector2(trans.position.x,trans.position.y) + portalOffset, 
            portalRot, portalDuration);
    }
}
