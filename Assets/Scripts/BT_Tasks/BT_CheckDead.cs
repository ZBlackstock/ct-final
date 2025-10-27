using BehaviorDesigner.Runtime.Tasks;
using System.Net;
using UnityEngine;

// Enemy AI - Check if player as killed enemy
public class BT_Check_Dead : EnemyConditional
{
    [SerializeField] private GameObject enableOnDeath;
    [SerializeField] private UI_Healthbar playerHealthbar;
    [SerializeField] private UI_HealthbarEnemy enemyHealthbar;
    bool isDead;

    public override TaskStatus OnUpdate()
    {
        if (enableOnDeath != null)
        {
            enableOnDeath.SetActive(anim.GetBool("dead"));
        }

        isDead = anim.GetBool("dead");

        if (isDead)
        {
            rb.velocity = Vector2.zero;
            enemyHealthbar.gameObject.SetActive(false);
            playerHealthbar.gameObject.SetActive(false);
        }

        return isDead ? TaskStatus.Failure : TaskStatus.Success;
    }
}
