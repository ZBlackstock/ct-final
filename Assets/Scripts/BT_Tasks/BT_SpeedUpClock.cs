using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy AI - calls SpeedUp() in small clock hand Rotate
public class BT_SpeedUpClock : EnemyAction
{
    public float speedRate;
    public Rotate clockRotate;

    public override void OnStart()
    {
        clockRotate.SpeedUp(speedRate);
    }
}
