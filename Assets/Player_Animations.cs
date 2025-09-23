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
}
