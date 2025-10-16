using BehaviorDesigner.Runtime.Tasks;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BT_MoveToPlayer : EnemyAction
{
    [SerializeField] private float moveSpeed;
    public float runSpeed = 200;
    public float walkSpeed = 50;
    public const float walkingRange = 30;
    public const float stopRange = 10f;
    public const float attackRange = 20f;
    public Vector2 attackWait = new Vector2(0.25f, 1f);
    public float attackWaitTimer;

    public bool reverseWalkWhenClose;
    [Range(0f, 1f)] public float reverseWalkChance = 0.3f;
    public Vector2 reverseWalkDurationRange = new Vector2(1f, 2f);
    private float reverseWalkDuration;
    public int reverseMultiplier = 1;
    [SerializeField] private bool forcedReverse;

    public override void OnStart()
    {
        attackWaitTimer = Random.Range(attackWait.x, attackWait.y);
        anim.SetInteger("attackSeq", -1);
    }

    public override TaskStatus OnUpdate()
    {
        if (anim.GetBool("dead"))
        {
            return TaskStatus.Failure;
        }

        anim.ResetTrigger("hit"); // Player is moving, so cannot be hit (will always block)
        InverseMoveSpeedCheck();
        CalculateMoveSpeed();
        SetAnimatorVariables();

        reverseWalkDuration -= Time.deltaTime;
        attackWaitTimer -= Time.deltaTime;

        if (Mathf.Abs(moveSpeed) == Mathf.Abs(walkSpeed))
        {
            if (reverseWalkDuration < 0)
            {
                ReverseWalk(StartReverseWalk());
            }
        }
        else if (!forcedReverse)
        {
            reverseMultiplier = 1;
        }

        if (PlayerWithinAttackRange() && !anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("counter"))
        {
            if (attackWaitTimer < 0)
            {
                attackWaitTimer = Random.Range(attackWait.x, attackWait.y);

                if (Attack())
                {
                    moveSpeed = 0;
                    return TaskStatus.Success;
                }
            }
        }

        return TaskStatus.Running;
    }

    public override void OnFixedUpdate()
    {
        rb.velocity = new Vector2(reverseMultiplier * moveSpeed, rb.velocity.y);
    }

    private void CalculateMoveSpeed()
    {
        switch (Vector2.Distance(playerController.transform.position, transform.position))
        {
            case < stopRange:
                if (!forcedReverse)
                {
                    ReverseWalk(true);
                    forcedReverse = true;
                }
                break;
            case >= walkingRange:
                moveSpeed = runSpeed;

                break;
            case < walkingRange:
                moveSpeed = walkSpeed;
                forcedReverse = false;
                break;
        }
    }

    private void InverseMoveSpeedCheck()
    {
        if (playerController.transform.position.x < transform.position.x && moveSpeed > 0 ||
            playerController.transform.position.x > transform.position.x && moveSpeed < 0)
        {
            runSpeed = -runSpeed;
            walkSpeed = -walkSpeed;
        }
    }

    private bool StartReverseWalk()
    {
        float chance = Random.Range(0f, 1f);
        return chance <= reverseWalkChance ? true : false;
    }

    private void ReverseWalk(bool reverse)
    {
        reverseMultiplier = reverse ? -1 : 1;
        reverseWalkDuration = Random.Range(reverseWalkDurationRange.x, reverseWalkDurationRange.y);
        reverseWalkDuration = reverse ? reverseWalkDuration : reverseWalkDuration * 2;
    }
    private void SetAnimatorVariables()
    {
        //Change to dynamic
        anim.SetInteger("moveSpeed", (int)(moveSpeed * reverseMultiplier));
    }

    private bool PlayerWithinAttackRange()
    {
        return Vector2.Distance(playerController.transform.position, transform.position) < attackRange;
    }

    private bool Attack()
    {
        float chance = 0.5f;
        return Random.Range(0f, 1f) < chance;
    }

    public override void OnEnd()
    {
       rb.velocity = Vector2.zero;
    }
}
