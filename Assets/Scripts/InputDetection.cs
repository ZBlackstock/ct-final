using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetection : MonoBehaviour
{
    public enum InputSource { keyboard, xbox };
    public InputSource currentInput;
    private PlayerController playerController;


    void Start()
    {
        currentInput = InputSource.keyboard;
        playerController = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (playerController.GetMoveInput() != 0)
        {
            if (IsUsingKeyboard())
            {
                currentInput = InputSource.keyboard;
            }
            else
            {
                currentInput = InputSource.xbox;
            }
        }

        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }

    }

    public bool IsUsingKeyboard()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool MouseClick()
    {
        if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetCurrentInput()
    {
        return currentInput.ToString();
    }
}