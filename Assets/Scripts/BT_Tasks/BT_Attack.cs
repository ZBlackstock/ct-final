using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using BehaviorDesigner.Runtime;
using System.Collections;

public class BT_Attack : EnemyAction
{
    public int attackSeq = -1;

    public float attackForceDelayTimer = 0.1f;
    public float attackForceDelay = 0.1f;
    public Vector2 attackForce, counteredForce;
    public bool success, addedForce, attackCountered;
    private float timer;
    public GameObject collision;
    public float collisionDuration = 0.1f;
    public float collisionTimer;
    private Enemy_Health health;
    [Range(0, 1)] private float counter_NormalisedTime; //When countered, skip to specific fram - makes counter connection look better

    public override void OnAwake()
    {
        health = gameObject.GetComponentInChildren<Enemy_Health>();
        base.OnAwake();
    }
    public override void OnStart()
    {
        rb.velocity = Vector2.zero;
        attackForceDelayTimer = attackForceDelay;
        collisionTimer = collisionDuration;
        anim.SetInteger("attackSeq", attackSeq);
        collision.SetActive(false);
        Debug.Log("Starting attack sequence");
        timer = 0;
        success = false;
        attackCountered = false;
    }

    public override TaskStatus OnUpdate()
    {
        // If timer above 0, decrease. Else set to -1 (add attack force)
        attackForceDelayTimer = attackForceDelayTimer >= 0 ? attackForceDelayTimer -= Time.deltaTime : -1;

        // Add force, then wait until anim is finished before returning success
        if (attackForceDelayTimer == -1 && !addedForce && !health.GetHasBeenCountered())
        {
            Attack();
            StartCoroutine(Wait());

            addedForce = true;
        }
        else
        {
            timer += Time.deltaTime;
        }

        CollisionDisable();

        //check not been countered
        if (health.GetHasBeenCountered() && !attackCountered)
        {
            AttackCountered();
            StopCoroutine(Wait());
            StartCoroutine(Wait());
            attackCountered = true;
        }

        return success ? TaskStatus.Success : TaskStatus.Running;
    }

    public void Attack()
    {
        rb.AddForce(attackForce * -transform.localScale.x, ForceMode2D.Impulse);
        collision.SetActive(true);

        anim.SetInteger("attackSeq", -1);
    }

    public void AttackCountered()
    {
        attackForceDelayTimer = 100;
        collision.SetActive(false);
        anim.SetInteger("attackSeq", -1);
        rb.AddForce(counteredForce * transform.localScale.x, ForceMode2D.Impulse);
    }

    public void CollisionDisable()
    {
        if (collision.activeSelf)
        {
            collisionTimer -= Time.deltaTime;
            if (collisionTimer <= 0)
            {
                collision.SetActive(false);
                collisionTimer = -1;
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        success = true;
    }

    public override void OnEnd()
    {
        success = false;
        addedForce = false;
        attackCountered = false;
        collision.SetActive(false);
        attackForceDelayTimer = attackForceDelay;
        collisionTimer = collisionDuration;
    }

    public string GetCounterAnimName()
    {
        if (collision.CompareTag("enemy_overhead"))
        {
            return "GravetenderKnight_Countered_Overhead";
        }
        else if (collision.CompareTag("enemy_uppercut"))
        {
            return "GravetenderKnight_Countered_Uppercut";
        }
        else
        {
            return "";
        }
    }
}
