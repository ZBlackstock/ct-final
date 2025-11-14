using BehaviorDesigner.Runtime.Tasks;
using System.Net;
using UnityEngine;

// Enemy AI - Check if player as killed enemy
public class BT_Check_Dead : EnemyConditional
{
    [SerializeField] private GameObject enableOnDeath;
    [SerializeField] private UI_Healthbar playerHealthbar;
    [SerializeField] private UI_HealthbarEnemy enemyHealthbar;
    [SerializeField] private BoxCollider2D musicTrigger; // Shrink on death to fade out music
    [SerializeField] private Rotate clockRotate;
    public bool shakeCamera;
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

            if(musicTrigger != null)
            {
                musicTrigger.size = Vector2.zero;   
            }
            if(clockRotate != null)
            {
                clockRotate.SpeedUp(-0.5f);
            }
            if (shakeCamera)
            {
                container.camShake.ShakeCamera(10);
            }
        }

        return isDead ? TaskStatus.Failure : TaskStatus.Success;
    }
}
