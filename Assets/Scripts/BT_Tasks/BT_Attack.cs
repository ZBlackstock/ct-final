using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using UnityEngine;

// Enemy AI - handles attacks
// Collisions must be set via enabling/disabling gameobject under AttackCollisions in hierarchy, via associated attack animation
public class BT_Attack : EnemyAction
{
    public int attackSeq = -1; // Tells anim int "attackSeq" what attack anim clip to play
    public float attackForceDelay = 0.1f; // Delay before adding velocity to enemy
    public Vector2 attackForce; // Velocity of attack
    public Vector2 counteredForce; // Velocity added when countered by player
    public float attackForceDuration = 0.01f; // Duration of time the enemy is moving forward when attacking
    public float counteredForceDuration = 0.01f; // Duration of time enemy is moving back when countered
    public SharedBool hit; // Global bool - has enemy been hit. used to cause quicker animation exit, or result in return hit
    public SharedBool returnHit; // Global bool, if true on exit, enemy will jab player
    public int attackStartAudioIndex = -1; // references index of SoundManager.enemy_Vocal_AttackStart[]
    public int attackEndAudioIndex = -1;// same for SoundManager.enemy_Vocal_AttackEnd[]

    private Enemy_Health health;
    private AnimatorStateInfo animStateInfo;
    [HideInInspector] public float attackForceDelayTimer = 0.1f;
    private float counteredTimer = -1;
    private bool success;
    private bool addedForce;
    private bool attackCountered;
    private SoundManager sound;

    public override void OnAwake()
    {
        health = gameObject.GetComponentInChildren<Enemy_Health>();
        sound = GameObject.FindFirstObjectByType<SoundManager>();
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
        returnHit.Value = false;
        if(attackStartAudioIndex != -1)
        {
            sound.PlaySound(sound.enemy_vocal_AttackStart[attackStartAudioIndex]);
        }
    }

    public override TaskStatus OnUpdate()
    {
        animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (counteredTimer > 0)
        {
            counteredTimer -= Time.deltaTime;
        }
        else
        {
            counteredTimer = -1;
        }

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

        // Check if player has hit enem during attack
        HitCheck(attackCountered);

        return success ? TaskStatus.Success : TaskStatus.Running;
    }

    public void Attack()
    {
        StartCoroutine(ChangeVelocity(attackForce, attackForceDuration));
        if (attackEndAudioIndex != -1)
        {
            sound.PlaySound(sound.enemy_vocal_AttackEnd[attackEndAudioIndex]);
        }
        anim.SetInteger("attackSeq", -1);
    }

    // Player countered enemy attack
    public IEnumerator AttackCountered()
    {
        attackForceDelayTimer = 100;
        anim.SetInteger("attackSeq", -1);
        StopCoroutine(ChangeVelocity(attackForce, attackForceDuration)); // Stop moving forward with attack force
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
        counteredTimer = animStateInfo.length - (animStateInfo.normalizedTime * animStateInfo.length);
        yield return new WaitUntil(() => counteredTimer == -1);
        ExitAttackTask();
    }

    // Check if enemy has been hit, and if it's following being countered
    public void HitCheck(bool attackCountered)
    {
        if (hit.Value == true)
        {
            ExitAttackTask();
            hit.Value = false;

            returnHit.Value = !attackCountered;
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
