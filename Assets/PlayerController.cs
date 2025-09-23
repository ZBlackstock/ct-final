using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 600;

    [SerializeField] private int uppercutIdleForce = 300;
    [SerializeField] private int uppercutSprintForce = 600;
    private int uppercutForce = 300;
    private bool uppercut;

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


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animVariables = bodyAnim.GetComponent<Player_Animations>();
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
    }

    void Update()
    {
        canmove = canMove();

        if (Input.GetKeyDown(KeyCode.C) && canMove())
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
        else if (Input.GetKeyDown(KeyCode.X) && canMove())
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

        if (canMove())
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            moveInput = 0;
        }

        bodyAnim.SetFloat("moveInput", moveInput);
        bodyAnim.SetBool("uppercut", uppercut);
        bodyAnim.SetBool("attack", attack);

        if ((faceRight == false && moveInput > 0 || faceRight == true && moveInput < 0))
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (canMove())
        {
            Vector2 targetVel = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            Vector2 velocityChange = targetVel - rb.velocity;
            rb.AddForce(velocityChange, ForceMode2D.Impulse);
        }
        else
        {
            if (uppercut)
            {
                uppercut = false;
                rb.velocity = Vector2.zero;
            }
            if (animVariables.GetUppercut())
            {
                rb.AddForce(new Vector2(uppercutForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
            }

            if (animVariables.GetUppercutStepBack())
            {
                rb.AddForce(new Vector2(-uppercutForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
            }

            if (attack)
            {
                attack = false;
                rb.velocity = Vector2.zero;
            }
            if (animVariables.GetAttack())
            {
                rb.AddForce(new Vector2(attackForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
            }

            if (animVariables.GetAttackKnockback())
            {
                rb.AddForce(new Vector2(-attackForce * direction * Time.fixedDeltaTime, rb.velocity.y), ForceMode2D.Impulse);
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
