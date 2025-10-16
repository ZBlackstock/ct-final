using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int uppercutKnockbackForce = 300;
    private int uppercutForce = 300;
    [SerializeField] private float uppercutForceDuration = 0.1f;
    [HideInInspector] public bool uppercut, uppercutKnockback;
    [SerializeField] private float uppercutInputWait = 0.1f;
    private float uppercutInputTimer;

    [Header("Step Counter")]
    [SerializeField] private int stepIdleForce = 200;
    [SerializeField] private int stepSprintForce = 300;
    private int stepForce = 200;
    public int stepKnockbackForce = 300;
    [SerializeField] private float stepForceDuration = 0.1f;
    [SerializeField] private float stepInputWait = 0.1f;
    private float stepInputTimer;
    [HideInInspector] public bool step, stepKnockback;


    [Header("Attack")]
    [SerializeField] private int attackIdleForce = 300;
    [SerializeField] private int attackSprintForce = 600;
    [SerializeField] private float attackForceDuration;
    private int attackForce = 300;
    private bool attack;
    private bool attack1;
    private bool canJumpAttack;
    [SerializeField] private float attackInputWait = 0.1f;
    private float attackInputTimer;

    [Header("AttackKnockback")]
    public bool attackKnockback;

    public Vector2 hurtKnockbackForce;
    [SerializeField] private int counteredForce = 300;
    public float maxHurtYVelocity = 1000;


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
    private SoundManager sound;
    private float startTimer = 9f;
    private Player_Health health;
    private bool disableMove;
    private bool UIOpen;
    private bool velocityCoroutineRunning;
    private bool playedLandSound;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip land, uppercutWhoosh, stepCounterChime, stepCounterSuccess, uppercutCounterSuccess;
    [SerializeField] private AudioClip[] attackWhoosh;
    [SerializeField] private AudioClip[] deflected;

    _ParticlesManager playerParticles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animVariables = bodyAnim.GetComponent<Player_Animations>();
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
        health = GetComponentInChildren<Player_Health>();
        playerParticles = GetComponent<_ParticlesManager>();
        sound = FindFirstObjectByType<SoundManager>();
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

        startTimer = !ignoreWakeUp ? startTimer -= Time.deltaTime : startTimer = -1;

        CheckInput();
        ExecuteInputs();
        SetAnimatorParameters();
        SubtractTimers();
        DetectAppropriateGravScale();

        if (groundCheck.isGrounded)
        {
            canJumpAttack = true;

            if (!playedLandSound && startTimer < 0)
            {
                sound.PlaySound(land);
                playedLandSound = true;
            }
        }
        else
        {
            playedLandSound = false;
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
            else if (attackInputTimer > 0 && (canMove() ||
                (bodyAnimState.IsName("Player_Attack") && bodyAnimState.normalizedTime > 0.6f && bodyAnimState.normalizedTime < 0.85f && !bodyAnim.IsInTransition(0))))
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
            if (canMove() && !velocityCoroutineRunning)
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, (pressJump ? jumpForce : rb.velocity.y));
                if (pressJump)
                {
                    pressJump = false;
                    sound.PlaySound(jump);
                }
            }
            else
            {
                if (!velocityCoroutineRunning)
                {
                    rb.velocity = Vector2.zero;
                }

                if (uppercut)
                {
                    ChangeVelocity(new Vector2(uppercutForce, 0), uppercutForceDuration);

                    sound.PlaySound(uppercutWhoosh);
                    playerParticles.uppercutParticles.transform.localScale = new Vector2(faceRight ? 1 : -1, 1);
                    playerParticles.PlayParticlesFromParticleSystem(playerParticles.uppercutParticles);
                    uppercut = false;
                    uppercutKnockback = true;
                }
                else if (animVariables.GetUppercutStepBack() && uppercutKnockback)
                {
                    ChangeVelocity(new Vector2(-uppercutKnockbackForce, 0), uppercutForceDuration);
                    sound.PlaySound(uppercutCounterSuccess);
                    uppercutKnockback = false;
                }
                else if (step)
                {
                    ChangeVelocity(new Vector2(stepForce, 0), stepForceDuration);
                    sound.PlaySound(stepCounterChime);

                    step = false;
                }
                else if (stepKnockback) // Step counter success
                {
                    ChangeVelocity(new Vector2(-stepForce, 0), stepForceDuration);
                    sound.PlaySound(stepCounterSuccess);
                    stepKnockback = false;
                }
                else if (attack || attack1)
                {
                    ChangeVelocity(new Vector2(attackForce, 0), attackForceDuration);
                    sound.PlaySound(attackWhoosh[attack ? 0 : 1]);

                    attack = false;
                    attack1 = false;
                }
                else if (attackKnockback)
                {
                    ChangeVelocity(new Vector2(-attackForce, 0), attackForceDuration);
                    attackKnockback = false;
                }
                else if (countered)
                {
                    ChangeVelocity(new Vector2(-counteredForce, 0), 0.1f);
                    sound.PlaySoundRandom(deflected, 1, 1, 1);
                    countered = false;
                }
            }
        }
        else // player is taking damage
        {
            rb.AddForce(hurtKnockbackForce, ForceMode2D.Impulse);
            if (rb.velocity.y > maxHurtYVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxHurtYVelocity);
            }

            animVariables.Hurt();
            bodyAnim.SetBool("hurt", false);
            health.SetHurt(false);
        }
    }

    private void ChangeVelocity(Vector2 velocity, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeVelocityCoroutine(velocity, duration));
    }

    private IEnumerator ChangeVelocityCoroutine(Vector2 velocity, float duration)
    {
        velocityCoroutineRunning = true;
        rb.velocity = new Vector2(velocity.x * direction, velocity.y);
        yield return new WaitForSeconds(duration);
        rb.velocity = Vector2.zero;
        velocityCoroutineRunning = false;
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
