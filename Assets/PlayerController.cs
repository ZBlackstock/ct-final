using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 600;

    [Header("Jumping")]
    [SerializeField] private int jumpForce = 6000;
    private bool pressJump;
    [SerializeField] private float groundedGravity;
    [SerializeField] private float jumpingGravity;

    [Header("Uppercut Counter")]
    [SerializeField] private int uppercutIdleForce = 300;
    [SerializeField] private int uppercutSprintForce = 600;
    private int uppercutForce = 300;
    private bool uppercut;

    [Header("Attack")]
    [SerializeField] private int attackIdleForce = 300;
    [SerializeField] private int attackSprintForce = 600;
    private int attackForce = 300;
    private bool attack;

    private float moveInput;
    private int direction = 1; // 1 = Right // -1 = left
    private Rigidbody2D rb;
    [SerializeField] private Animator bodyAnim;
    private AnimatorStateInfo bodyAnimState;
    private bool faceRight = true;
    private Player_Animations animVariables;
    private bool canmove; // just for debug inspector

    [SerializeField] private GroundCheck groundCheck;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animVariables = bodyAnim.GetComponent<Player_Animations>();
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
    }

    void Update()
    {
        canmove = canMove();

        if (Input.GetKeyDown(KeyCode.C) && canMove() && groundCheck.isGrounded)
        {
            if (bodyAnimState.IsName("Player_Run"))
            {
                uppercutForce = uppercutSprintForce;
            }
            else
            {
                uppercutForce = uppercutIdleForce;
            }

            uppercut = true;
        }
        else if (Input.GetKeyDown(KeyCode.X) && canMove() && groundCheck.isGrounded)
        {
            if (bodyAnimState.IsName("Player_Run"))
            {
                attackForce = attackSprintForce;
            }
            else
            {
                attackForce = attackIdleForce;
            }

            attack = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canMove() && groundCheck.isGrounded)
        {
            pressJump = true;
        }

        if (canMove())
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            moveInput = 0;
        }

        if ((faceRight == false && moveInput > 0 || faceRight == true && moveInput < 0))
        {
            Flip();
        }

        if (groundCheck.isGrounded)
        {
            rb.gravityScale = groundedGravity;
        }
        else
        {
            rb.gravityScale = jumpingGravity;
        }

        bodyAnim.SetFloat("moveInput", moveInput);
        bodyAnim.SetBool("uppercut", uppercut);
        bodyAnim.SetBool("attack", attack);
        bodyAnim.SetBool("isGrounded", groundCheck.isGrounded);
    }

    private void FixedUpdate()
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
                rb.AddForce(new Vector2(attackForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
            }
            else if (animVariables.GetAttackKnockback())
            {
                rb.AddForce(new Vector2(-attackForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    public void Flip()
    {
        faceRight = !faceRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        direction *= -1;
    }

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
