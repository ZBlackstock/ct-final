using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour
{
    private CameraShake cameraShake;
    private Animator playerAnim;
    private AnimatorStateInfo playerStateInfo;  
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private GameObject[] attackCollisions;

    public void Hurt()
    {
        uppercutStepback_False();
        uppercut_False();
        attack_False();
        attackKnockback_False();
        step_False();
    }

    private void Awake()
    {
        cameraShake = FindFirstObjectByType<CameraShake>();
        playerAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        DisableAllAttackCollisions();
    }

    public void DisableAllAttackCollisions()
    {
        foreach (GameObject col in attackCollisions)
        {
            col.SetActive(false);
        }
    }

    private void Update()
    {
        playerStateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
    }

    public void ShakeCamera()
    {
        cameraShake.ShakeCamera(shakeDuration);
    }

    ///////////////////////////////////////////

    private bool uppercutStepBack;

    public void uppercutStepback_True()
    {
        uppercutStepBack = true;
    }

    public void uppercutStepback_False()
    {
        uppercutStepBack = false;
    }

    public bool GetUppercutStepBack()
    {
        return uppercutStepBack;
    }

    ///////////////////////////////////////////

    private bool uppercut;

    public void uppercut_True()
    {
        uppercut = true;
    }

    public void uppercut_False()
    {
        uppercut = false;
    }

    public bool GetUppercut()
    {
        return uppercut;
    }

    ///////////////////////////////////////////

    private bool attack;

    private bool countered;

    public void countered_True()
    {
        countered = true;
    }

    public void countered_False()
    {
        countered = false;
    }

    public bool GetCountered()
    {
        return countered;
    }

    public void attack_True()
    {
        attack = true;
    }

    private void AttackHitbox_True()
    {
        if (playerStateInfo.IsName("Player_Attack"))
        {
            attackCollisions[0].SetActive(true);
        }
        else if (playerStateInfo.IsName("Player_Attack1"))
        {
            attackCollisions[1].SetActive(true);
        }
        else if (playerStateInfo.IsName("Player_JumpAttack"))
        {
            attackCollisions[2].SetActive(true);
        }
    }

    private void AttackHitbox_False()
    {
        if (playerStateInfo.IsName("Player_Attack"))
        {
            attackCollisions[0].SetActive(false);
        }
        else if (playerStateInfo.IsName("Player_Attack1"))
        {
            attackCollisions[1].SetActive(false);
        }
        else if (playerStateInfo.IsName("Player_JumpAttack"))
        {
            attackCollisions[2].SetActive(false);
        }
    }

    public void attack_False()
    {
        attack = false;
    }

    public bool GetAttack()
    {
        return attack;
    }

    ///////////////////////////////////////////

    private bool attackKnockback;

    public void attackKnockback_True()
    {
        attackKnockback = true;
    }

    public void attackKnockback_False()
    {
        attackKnockback = false;
    }

    public bool GetAttackKnockback()
    {
        return attackKnockback;
    }

    ///////////////////////////////////////////

    private bool step;

    public void step_True()
    {
        step = true;
    }

    public void step_False()
    {
        step = false;
    }

    public bool GetStep()
    {
        return step;
    }
    ///////////////////////////////////////////

    private bool stepKnockback;

    public void stepKnockback_True()
    {
        stepKnockback = true;
    }

    public void stepKnockback_False()
    {
        stepKnockback = false;
    }

    public bool GetStepKnockback()
    {
        return stepKnockback;
    }
}
