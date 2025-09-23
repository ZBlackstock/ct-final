using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour
{
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

    public void attack_True()
    {
        attack = true;
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
}
