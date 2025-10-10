using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class GravetenderKnight : MonoBehaviour
{
    private PlayerController playerController;
    private Transform playerTrans;
    [SerializeField] private Animator bodyAnim;
    private AnimatorStateInfo bodyAnimState;
    private bool faceRight;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        playerTrans = playerController.GetComponent<Transform>();
    }

    void Update()
    {
        FlipCheck();
        bodyAnim.SetBool("faceRight", faceRight);
    }

    private void FlipCheck()
    {
        if (playerTrans.position.x < transform.position.x && faceRight ||
            playerTrans.position.x > transform.position.x && !faceRight)
        {
            if (CanMove())
            {
                Flip();
            }
        }
    }

    public void Flip()
    {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

        faceRight = !faceRight;
    }

    public bool CanMove()
    {
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
        if (bodyAnimState.IsName("GravetenderKnight_Idle") || bodyAnimState.IsName("GravetenderKnight_Run") ||
            bodyAnimState.IsName("GravetenderKnight_Walk_Forward") || bodyAnimState.IsName("GravetenderKnight_Walk_Backward") || 
            bodyAnimState.IsName("GravetenderKnight_Counter"))
        {
            return true;
        }

        return false;
    }
}
