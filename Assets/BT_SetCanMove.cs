using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class BT_SetCanMove : MonoBehaviour
{
    BehaviorTree behaviourTree;
    private Container container;

    void Awake()
    {
        container = FindFirstObjectByType<Container>();
        behaviourTree = GetComponent<BehaviorTree>();
    }

    void Update()
    {
        behaviourTree.SetVariableValue("canMove", container.enemy.CanMove());
    }
}
