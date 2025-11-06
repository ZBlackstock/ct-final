using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy AI - handles attacks
// Collisions must be set via enabling/disabling gameobject under AttackCollisions in hierarchy, via associated attack animation
public class BT_Attack : EnemyAction
{
    private Container container;

    public int attackSeq = -1; // Tells anim int "attackSeq" what attack anim clip to play
    [Header("Attack Velocity Changes")]
    public List<Vector2> attackForces = new List<Vector2>(1); // Velocity X, Velocity Y, Duration, WaitTime
    public List<float> attackDurations = new List<float>(1); // Velocity durations
    public List<float> attackWaits = new List<float>(1); // Time between velocity changes
    private int iterator = 0;

    [Header("Countered Velocity Change")]
    public Vector2 counteredForce; // Velocity added when countered by player
    public float counteredForceDuration = 0.01f; // Duration of time enemy is moving back when countered

    [Header("Global Variables")]
    public SharedBool hit; // Global bool - has enemy been hit. used to cause quicker animation exit, or result in return hit
    public SharedBool returnHit; // Global bool, if true on exit, enemy will jab player

    [Header("Enemy Vocals Audio")]
    public int attackStartAudioIndex = -1; // references index of SoundManager.enemy_Vocal_AttackStart[]
    public int attackEndAudioIndex = -1;// same for SoundManager.enemy_Vocal_AttackEnd[]

    private AnimatorStateInfo animStateInfo;
    [HideInInspector] public float attackForceDelayTimer = 0.1f;
    private float counteredTimer = -1;
    private float velocityTimer = -1;
    private bool success;
    private bool addedForce;
    private bool attackCountered;

    public override void OnAwake()
    {
        container = GameObject.FindFirstObjectByType<Container>();
        base.OnAwake();
    }

    public override void OnStart()
    {
        iterator = 0;
        rb.velocity = Vector2.zero;
        attackForceDelayTimer = attackWaits[iterator];
        anim.SetInteger("attackSeq", attackSeq);
        success = false;
        attackCountered = false;
        hit.Value = false;
        anim.ResetTrigger("hit");
        returnHit.Value = false;
        if (attackStartAudioIndex != -1)
        {
            container.sounds.PlaySound(container.sounds.enemy_vocal_AttackStart[attackStartAudioIndex]);
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
        if (velocityTimer > 0)
        {
            velocityTimer -= Time.deltaTime;
        }
        else
        {
            velocityTimer = -1;
        }

        // If timer above 0, decrease. Else set to -1 (add attack force)
        attackForceDelayTimer = attackForceDelayTimer >= 0 ? attackForceDelayTimer -= Time.deltaTime : -1;

        // Add force, then wait until anim is finished before returning success
        if (attackForceDelayTimer == -1 && !addedForce && !container.enemyHealth.GetHasBeenCountered())
        {
            Attack();
            StartCoroutine(Wait());
        }

        //check not been countered
        if (container.enemyHealth.GetHasBeenCountered() && !attackCountered)
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
        Debug.Log("Attack");
        StartCoroutine(ChangeVelocity(attackForces[iterator], attackDurations[iterator]));

        if (attackForces.Count > iterator + 1)
        {
            iterator++;
            attackForceDelayTimer = attackWaits[iterator];
        }
        else
        {
            addedForce = true;
        }

        if (attackEndAudioIndex != -1)
        {
            container.sounds.PlaySound(container.sounds.enemy_vocal_AttackEnd[attackEndAudioIndex]);
        }
        anim.SetInteger("attackSeq", -1);
    }

    // Player countered enemy attack
    public IEnumerator AttackCountered()
    {
        attackForceDelayTimer = 100;
        anim.SetInteger("attackSeq", -1);
        StopCoroutine(ChangeVelocity(attackForces[iterator], attackDurations[iterator])); // Stop moving forward with attack force
        StartCoroutine(ChangeVelocity(-counteredForce, counteredForceDuration));
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector2.zero;
    }

    private IEnumerator ChangeVelocity(Vector2 velocity, float duration)
    {
        Debug.Log("ChangeVelocity");
        rb.velocity = new Vector2(velocity.x * -transform.localScale.x, velocity.y);
        velocityTimer = duration;
        yield return new WaitUntil(() => velocityTimer == -1);
        rb.velocity = Vector2.zero;
    }

    IEnumerator Wait()
    {
        yield return null; // Wait frame to ensure correct animation clip
        counteredTimer = animStateInfo.length - (animStateInfo.normalizedTime * animStateInfo.length);
        yield return new WaitUntil(() => counteredTimer == -1);
        ExitAttackTask();
        Debug.Log("success");
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
        iterator = 0;
        anim.ResetTrigger("hit");
        StopAllCoroutines();
        success = false;
        addedForce = false;
        attackCountered = false;
        hit.Value = false;
        attackForceDelayTimer = attackWaits[iterator];
        anim.SetInteger("attackSeq", -1);
    }
}
