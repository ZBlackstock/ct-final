using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class GravetenderKnight_BT : MonoBehaviour
{
    BehaviorTree behaviourTree;
    private PlayerController playerController;
    private Transform playerTrans;

    public float distanceToPlayer;

    void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        playerTrans = playerController.GetComponent<Transform>();
        behaviourTree = GetComponent<BehaviorTree>();
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(playerTrans.position, transform.position);
        behaviourTree.SetVariableValue("distanceToPlayer", distanceToPlayer);
    }
}
