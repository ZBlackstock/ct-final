using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class Enemy : MonoBehaviour
{
    private PlayerController playerController;
    private Transform playerTrans;
    [SerializeField] private Animator bodyAnim;
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
        string clipName = GetCurrentAnimClipName();
        if (clipName.Contains("Idle") || clipName.Contains("Run") || clipName.Contains("Walk") || clipName.Contains("CounterPlayer"))
        {
            return true;
        }

        return false;
    }

    public string GetCurrentAnimClipName()
    {
        if (!bodyAnim.IsInTransition(0))
        {
            animClipInfo = bodyAnim.GetCurrentAnimatorClipInfo(0);
        }
        else
        {
            animClipInfo = bodyAnim.GetNextAnimatorClipInfo(0);
        }

        string clipName = "";
        if (animClipInfo != null && animClipInfo.Length > 0)
        {
            clipName = animClipInfo[0].clip.name;
        }
        else
        {
            clipName = "NoClip";
        }
        return clipName;    
    }
}
