using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool ignoreWakeUp;
    public bool wakeUpKneel;
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
    private bool attack1;
    private bool canJumpAttack;
    [SerializeField] private float attackInputWait = 0.1f;
    private float attackInputTimer;

    [Header("AttackKnockback")]
    [SerializeField] private int attackKnockbackForce = -500;
    public bool attackKnockback;

    public Vector2 hurtKnockbackForce;
    [SerializeField] private int counteredForce = 300;

    public bool countered;

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
    private bool disableMove;
    private bool UIOpen;

    Player_Particles playerParticles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animVariables = bodyAnim.GetComponent<Player_Animations>();
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
        health = GetComponentInChildren<Player_Health>();
        playerParticles = GetComponent<Player_Particles>();

        bodyAnim.SetBool("wakeUpKneel", wakeUpKneel);
    }

    private void Start()
    {
        startTimer = wakeUpKneel ? 2.5f : 9f;
    }

    void Update()
    {
        canmove = canMove();
        moveInput = canMove() ? Input.GetAxisRaw("Horizontal") : 0;

        if (attack1)
        {
            attack1 = false;
        }

        startTimer = !ignoreWakeUp ? startTimer -= Time.deltaTime : startTimer = -1;

        CheckInput();
        ExecuteInputs();
        SetAnimatorParameters();
        SubtractTimers();
        DetectAppropriateGravScale();

        if (groundCheck.isGrounded)
        {
            canJumpAttack = true;
        }

        if (canMove())
        {
            animVariables.attackKnockback_False();
            animVariables.attack_False();
            animVariables.countered_False();
            animVariables.stepKnockback_False();
            animVariables.step_False();
            animVariables.uppercutStepback_False();
            animVariables.uppercut_False();
        }
    }


    private void SetAnimatorParameters()
    {
        bodyAnim.SetFloat("moveInput", moveInput);
        bodyAnim.SetFloat("startTimer", startTimer);
        bodyAnim.SetBool("uppercut", uppercut);
        bodyAnim.SetBool("step", step);
        bodyAnim.SetBool("attack", attack);
        bodyAnim.SetBool("attack1", attack1);
        bodyAnim.SetBool("isGrounded", groundCheck.isGrounded);
        bodyAnim.SetBool("countered", countered);
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
        if (!GetUIOpen())
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
            else if (attackInputTimer > 0 && (canMove() || bodyAnimState.IsName("Player_Attack") && bodyAnimState.normalizedTime > 0.6f))
            {
                if (groundCheck.isGrounded || !groundCheck.isGrounded && canJumpAttack)
                {
                    attackInputTimer = 0;
                    attackForce = moveInput != 0 ? attackSprintForce : attackIdleForce;

                    if (!(bodyAnimState.IsName("Player_Attack") && bodyAnimState.normalizedTime > 0.6f))
                    {
                        attack = true;
                    }
                    else
                    {
                        attack1 = true;
                    }

                    canJumpAttack = groundCheck.isGrounded;
                }
            }
            else if (stepInputTimer > 0 && canMove())
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

                    playerParticles.uppercutParticles.transform.localScale = new Vector2(faceRight ? 1 : -1, 1); ;
                    playerParticles.PlayeParticlesFromParticleSystem(playerParticles.uppercutParticles);
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
                else if (countered)
                {
                    countered = false;
                    rb.velocity = Vector2.zero;
                }

                if (animVariables.GetUppercut())
                {
                    rb.AddForce(new Vector2(uppercutForce * direction * Time.fixedDeltaTime, 0), ForceMode2D.Impulse);
                }
                else if (animVariables.GetUppercutStepBack())
                {
                    rb.AddForce(new Vector2(-uppercutForce * direction * Time.fixedDeltaTime, 0), ForceMode2D.Impulse);
                }
                else if (animVariables.GetAttack())
                {
                    if (!attackKnockback)
                    {
                        rb.AddForce(new Vector2(attackForce * direction * Time.fixedDeltaTime, -rb.velocity.y), ForceMode2D.Impulse);
                    }
                    else
                    {
                        Vector2 velocity = TakeVelocityAndSetZero();

                        rb.AddForce(new Vector2(-velocity.x * 100 * Time.fixedDeltaTime, -velocity.y), ForceMode2D.Impulse);
                        attackKnockback = false;
                    }
                }
                else if (animVariables.GetStep())
                {
                    rb.AddForce(new Vector2(stepForce * direction * Time.fixedDeltaTime, 0), ForceMode2D.Impulse);
                }
                else if (animVariables.GetCountered())
                {
                    rb.AddForce(new Vector2(-counteredForce * direction * Time.fixedDeltaTime, 0), ForceMode2D.Impulse);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
        else // player is taking damage
        {
            animVariables.Hurt();
            Vector2 velocity = TakeVelocityAndSetZero();
            Vector2 force = new Vector2(hurtKnockbackForce.x - velocity.x * Time.fixedDeltaTime, hurtKnockbackForce.y - velocity.y * Time.fixedDeltaTime);
            rb.AddForce(force, ForceMode2D.Impulse);
            bodyAnim.SetBool("hurt", false);
            health.SetHurt(false);
        }
    }


    private Vector2 TakeVelocityAndSetZero()
    {
        float x = rb.velocity.x;
        float y = rb.velocity.y;
        rb.velocity = Vector2.zero;

        return new Vector2(x, y);
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
    public bool canMove()
    {
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
        if (bodyAnimState.IsName("Player_Run") || bodyAnimState.IsName("Player_Idle") || bodyAnimState.IsName("Player_Jump"))
        {
            if (!disableMove && !UIOpen)
            {
                return true;
            }
        }

        return false;
    }

    public void SetDisableMove(bool disable)
    {
        disableMove = disable;
    }
    public void SetUIOpen(bool open, bool waitFrame)
    {
        if (!waitFrame)
        {
            UIOpen = open;
        }
        else
        {
            StartCoroutine(WaitFrame());
        }
    }

    private IEnumerator WaitFrame()
    {
        yield return null;
        SetUIOpen(false, false);
    }

    public bool GetUIOpen()
    {
        return UIOpen;
    }

    public float GetMoveInput()
    {
        return moveInput;
    }
}
