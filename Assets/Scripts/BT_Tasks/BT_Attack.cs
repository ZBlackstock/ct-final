using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;
using static BehaviorDesigner.Runtime.BehaviorManager;

public class BT_Attack : EnemyAction
{
    public int attackSeq = -1;

    public float attackForceDelayTimer = 0.1f;
    public float attackForceDelay = 0.1f;
    public Vector2 attackForce, counteredForce;
    public float attackForceDuration = 0.01f, counteredForceDuration = 0.01f;
    public bool success, addedForce, attackCountered;
    private Enemy_Health health;
    [Range(0, 1)] private float counter_NormalisedTime; //When countered, skip to specific fram - makes counter connection look better
    private bool velocityCoroutineRunning;

    public SharedBool hit;

    public override void OnAwake()
    {
        health = gameObject.GetComponentInChildren<Enemy_Health>();
        base.OnAwake();
    }

    public override void OnStart()
    {
        rb.velocity = Vector2.zero;
        attackForceDelayTimer = attackForceDelay;
        anim.SetInteger("attackSeq", attackSeq);
        success = false;
        attackCountered = false;
        hit.Value = false;
        anim.ResetTrigger("hit");
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

        //check not been countered
        if (health.GetHasBeenCountered() && !attackCountered)
        {
            StartCoroutine(AttackCountered());
            StopCoroutine(Wait());
            StartCoroutine(Wait());
            attackCountered = true;
        }

        // Check if counter has been executed
        if (attackCountered)
        {
            HitCheck();
        }

        return success ? TaskStatus.Success : TaskStatus.Running;
    }

    public void Attack()
    {
        StartCoroutine(ChangeVelocity(attackForce, attackForceDuration));
        anim.SetInteger("attackSeq", -1);
    }

    public IEnumerator AttackCountered()
    {
        attackForceDelayTimer = 100;
        anim.SetInteger("attackSeq", -1);
        StopCoroutine(ChangeVelocity(attackForce, attackForceDuration));
        StartCoroutine(ChangeVelocity(-counteredForce, counteredForceDuration));
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
    }

    private IEnumerator ChangeVelocity(Vector2 velocity, float duration)
    {
        rb.velocity = new Vector2(velocity.x * -transform.localScale.x, velocity.y);
        yield return new WaitForSeconds(duration);
        rb.velocity = Vector2.zero;
    }



    IEnumerator Wait()
    {
        yield return null; // Wait frame to ensure correct animation clip
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        ExitAttackTask();
    }

    public void HitCheck()
    {
        if (hit.Value == true)
        {
            ExitAttackTask();
            hit.Value = false;
            anim.ResetTrigger("hit");
        }
    }

    public void ExitAttackTask()
    {
        success = true;
    }

    public override void OnEnd()
    {
        anim.ResetTrigger("hit");
        StopAllCoroutines();
        success = false;
        addedForce = false;
        attackCountered = false;
        hit.Value = false;
        attackForceDelayTimer = attackForceDelay;
        anim.SetInteger("attackSeq", -1);
    }
}
