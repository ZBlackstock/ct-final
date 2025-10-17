using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputDetectChangeSprite : MonoBehaviour
{
    [SerializeField] private Sprite keys, controller;
    private SpriteRenderer sr;
    private Image img;
    private InputDetection input;
    [SerializeField] private Menu_InputDetection menu_input;

    private void Awake()
    {

        if (FindFirstObjectByType<InputDetection>())
        {
            input = FindFirstObjectByType<InputDetection>();
        }

        if (GetComponent<SpriteRenderer>())
        {
            sr = GetComponent<SpriteRenderer>();
        }
        else if (GetComponent<Image>())
        {
            img = GetComponent<Image>();
        }
    }

    void Update()
    {
        if (input)
        {
            GameplayInputSpriteCheck();
        }
        else
        {
            MenuInputSpriteCheck();
        }
    }

    private void GameplayInputSpriteCheck()
    {
        if (sr)
        {
            if (sr.sprite == keys && input.currentInput == InputDetection.InputSource.xbox ||
                sr.sprite == controller && input.currentInput == InputDetection.InputSource.keyboard)
            {
                sr.sprite = input.currentInput == InputDetection.InputSource.keyboard ? keys : controller;
            }
        }
        else if (img)
        {
            if (img.sprite == keys && input.currentInput == InputDetection.InputSource.xbox ||
                img.sprite == controller && input.currentInput == InputDetection.InputSource.keyboard)
            {
                img.sprite = input.currentInput == InputDetection.InputSource.keyboard ? keys : controller;
            }
        }
    }

    private void MenuInputSpriteCheck()
    {
        if (sr)
        {
            if (sr.sprite == keys && menu_input.currentInput == Menu_InputDetection.InputSource.xbox ||
                sr.sprite == controller && menu_input.currentInput == Menu_InputDetection.InputSource.keyboard)
            {
                sr.sprite = menu_input.currentInput == Menu_InputDetection.InputSource.keyboard ? keys : controller;
            }
        }
        else if (img)
        {
            if (img.sprite == keys && menu_input.currentInput == Menu_InputDetection.InputSource.xbox ||
                img.sprite == controller && menu_input.currentInput == Menu_InputDetection.InputSource.keyboard)
            {
                img.sprite = menu_input.currentInput == Menu_InputDetection.InputSource.keyboard ? keys : controller;
            }
        }
    }
}
