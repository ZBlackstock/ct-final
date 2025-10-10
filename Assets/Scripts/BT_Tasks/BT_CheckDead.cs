using BehaviorDesigner.Runtime.Tasks;
using System.Net;
using UnityEngine;

public class BT_Check_Dead : EnemyConditional
{
    [SerializeField] private GameObject enableOnDeath;
    public override TaskStatus OnUpdate()
    {
        if (enableOnDeath != null)
        {
            enableOnDeath.SetActive(anim.GetBool("dead"));
        }

        return anim.GetBool("dead") ? TaskStatus.Failure : TaskStatus.Success;
    }
}
