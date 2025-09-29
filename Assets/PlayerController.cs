using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool ignoreWakeUp;
    [SerializeField] private int moveSpeed = 600;

    [Header("Jumping")]
    [SerializeField] private int jumpForce = 6000;
    private bool pressJump;
    public float groundedGravity;
    public float jumpingGravity;
    [SerializeField] private float jumpInputWait = 0.1f;
    private float jumpInputTimer;

    [Header("Uppercut Counter")]
    [SerializeField] private int uppercutIdleForce = 300;
    [SerializeField] private int uppercutSprintForce = 600;
    private int uppercutForce = 300;
    private bool uppercut;
    [SerializeField] private float uppercutInputWait = 0.1f;
    private float uppercutInputTimer;

    [Header("Step Counter")]
    [SerializeField] private int stepIdleForce = 200;
    [SerializeField] private int stepSprintForce = 300;
    private int stepForce = 300;
    private bool step;
    [SerializeField] private float stepInputWait = 0.1f;
    private float stepInputTimer;

    [Header("Attack")]
    [SerializeField] private int attackIdleForce = 300;
    [SerializeField] private int attackSprintForce = 600;
    private int attackForce = 300;
    private bool attack;
    [SerializeField] private float attackInputWait = 0.1f;
    private float attackInputTimer;

    [Header("AttackKnockback")]
    [SerializeField] private int attackKnockbackForce = -500;
    public bool attackKnockback;

    public Vector2 hurtKnockbackForce;

    private float moveInput;
    private int direction = 1; // 1 = Right // -1 = left
    private Rigidbody2D rb;
    [SerializeField] private Animator bodyAnim;
    private AnimatorStateInfo bodyAnimState;
    public bool faceRight { get; private set; } = true;
    private Player_Animations animVariables;
    private bool canmove; // just for debug inspector

    [SerializeField] private GroundCheck groundCheck;
    private float startTimer = 9f;
    private Player_Health health;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animVariables = bodyAnim.GetComponent<Player_Animations>();
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
        health = GetComponentInChildren<Player_Health>();
    }

    void Update()
    {
        canmove = canMove();
        moveInput = canMove() ? Input.GetAxisRaw("Horizontal") : 0;

        startTimer = !ignoreWakeUp ? -Time.deltaTime : startTimer = -1;

        CheckInput();
        ExecuteInputs();
        SetAnimatorParameters();
        SubtractTimers();
        DetectAppropriateGravScale();
    }

    private void SetAnimatorParameters()
    {
        bodyAnim.SetFloat("moveInput", moveInput);
        bodyAnim.SetFloat("startTimer", startTimer);
        bodyAnim.SetBool("uppercut", uppercut);
        bodyAnim.SetBool("step", step);
        bodyAnim.SetBool("attack", attack);
        bodyAnim.SetBool("isGrounded", groundCheck.isGrounded);
    }

    private void SubtractTimers()
    {
        attackInputTimer -= Time.deltaTime;
        uppercutInputTimer -= Time.deltaTime;
        stepInputTimer -= Time.deltaTime;
        jumpInputTimer -= Time.deltaTime;
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            uppercutInputTimer = uppercutInputWait;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            attackInputTimer = attackInputWait;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            stepInputTimer = stepInputWait;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInputTimer = jumpInputWait;
        }
    }
    private void DetectAppropriateGravScale()
    {
        rb.gravityScale = groundCheck.isGrounded && !bodyAnimState.IsName("Player_Hurt") ? groundedGravity : jumpingGravity;
    }

    private void ExecuteInputs()
    {
        if (!health.IsHurt() && !health.IsDeath())
        {
            if (uppercutInputTimer > 0 && canMove() && groundCheck.isGrounded)
            {
                uppercutInputTimer = 0;
                uppercutForce = bodyAnimState.IsName("Player_Run") ? uppercutSprintForce : uppercutIdleForce;
                uppercut = true;
            }
            else if (attackInputTimer > 0 && canMove() && groundCheck.isGrounded)
            {
                attackInputTimer = 0;
                attackForce = bodyAnimState.IsName("Player_Run") ? attackSprintForce : attackIdleForce;
                attack = true;
            }
            else if (stepInputTimer > 0 && canMove() && groundCheck.isGrounded)
            {
                stepInputTimer = 0;
                stepForce = bodyAnimState.IsName("Player_Run") ? stepSprintForce : stepIdleForce;
                step = true;
            }
            if (jumpInputTimer > 0 && canMove() && groundCheck.isGrounded)
            {
                jumpInputTimer = 0;
                pressJump = true;
            }

            if ((faceRight == false && moveInput > 0 || faceRight == true && moveInput < 0))
            {
                Flip();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!health.IsHurt())
        {
            if (canMove())
            {
                Vector2 targetVel = new Vector2(moveInput * moveSpeed, rb.velocity.y);
                Vector2 velocityChange = targetVel - rb.velocity;
                rb.AddForce(velocityChange, ForceMode2D.Impulse);

                if (pressJump)
                {
                    pressJump = false;

                    rb.AddForce(new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime), ForceMode2D.Impulse);
                }
            }
            else
            {
                if (uppercut)
                {
                    uppercut = false;
                    rb.velocity = Vector2.zero;
                }
                else if (step)
                {
                    step = false;
                    rb.velocity = Vector2.zero;
                }
                else if (attack)
                {
                    attack = false;
                    rb.velocity = Vector2.zero;
                }

                if (animVariables.GetUppercut())
                {
                    rb.AddForce(new Vector2(uppercutForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
                }
                else if (animVariables.GetUppercutStepBack())
                {
                    rb.AddForce(new Vector2(-uppercutForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
                }
                else if (animVariables.GetAttack())
                {
                    if (!attackKnockback)
                    {
                        rb.AddForce(new Vector2(attackForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(new Vector2(attackKnockbackForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
                        attackKnockback = false;
                        print("attack knockback");
                    }
                }
                else if (animVariables.GetStep())
                {
                    rb.AddForce(new Vector2(stepForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
        else // player is taking damage
        {
            // Check if jumping, if so, subtract current y velocity
            Vector2 force = new Vector2(hurtKnockbackForce.x * Time.fixedDeltaTime,
                bodyAnimState.IsName("Player_Run") ? hurtKnockbackForce.y * Time.fixedDeltaTime :
                hurtKnockbackForce.y * Time.fixedDeltaTime - rb.velocity.y);
            rb.AddForce(force, ForceMode2D.Impulse);
            bodyAnim.SetBool("hurt", false);
            health.SetHurt(false);
        }
    }

    // Flip player scaling
    public void Flip()
    {
        faceRight = !faceRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        direction *= -1;
    }

    // Checks if player anim is currenty a movable state (not interacting or taking damage etc
    // Common check to see if player can perform action
    bool canMove()
    {
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
        if (bodyAnimState.IsName("Player_Run") || bodyAnimState.IsName("Player_Idle") || bodyAnimState.IsName("Player_Jump"))
        {
            return true;
        }

        return false;
    }
}
