using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_SetObjectActive : EnemyAction
{
    public GameObject setObject;
    public bool objectActive;

    public override void OnStart()
    {
        setObject.SetActive(objectActive);
    }
}
