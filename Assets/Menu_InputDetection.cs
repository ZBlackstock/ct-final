using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_InputDetection : MonoBehaviour
{
    public enum InputSource { keyboard, xbox };
    public InputSource currentInput;

    void Start()
    {
        currentInput = InputSource.keyboard;
    }

    void Update()
    {
        if (Input.GetButtonDown("A") || Input.GetButtonDown("B") || Input.GetButtonDown("X") || Input.GetButtonDown("Y") ||
         Input.GetButtonDown("RightBumper") || Input.GetButtonDown("LeftBumper") || Input.GetButtonDown("RightTrigger") ||
         Input.GetButtonDown("LeftTrigger") || Input.GetButtonDown("Start"))
        {
            currentInput = InputSource.xbox;
        }
        else if (Input.anyKeyDown)
        {
            currentInput = InputSource.keyboard;
        }

    }

    public bool IsUsingKeyboard()
    {
        return currentInput == InputSource.keyboard;
    }

    public string GetCurrentInput()
    {
        return currentInput.ToString();
    }
}