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
    private AnimatorClipInfo[] animClipInfo;
    private bool faceRight;
    private Enemy_Health health;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        playerTrans = playerController.GetComponent<Transform>();
        health = GetComponentInChildren<Enemy_Health>();
    }

    void Update()
    {
        FlipCheck();
        bodyAnim.SetBool("faceRight", faceRight);
        health.Set_FaceRight(faceRight);
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

    public bool CanMove()  // BE CAREFUL WITH NAMING ANIM CLIPS
    {
        bodyAnimState = bodyAnim.GetCurrentAnimatorStateInfo(0);
        animClipInfo = bodyAnim.GetCurrentAnimatorClipInfo(0);
        if (animClipInfo[0].clip.name.Contains("Idle") || animClipInfo[0].clip.name.Contains("Run") ||
            animClipInfo[0].clip.name.Contains("Walk") || animClipInfo[0].clip.name.Contains("_Counter"))
        {
            return true;
        }

        return false;
    }
}
