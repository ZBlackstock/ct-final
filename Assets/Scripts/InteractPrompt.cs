using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Sprite eKey, xButton;
    private PlayerController playerController;
    private GroundCheck groundCheck;
    private InputDetection input;
    private SpriteRenderer arrow, symbol;
    public bool playerInRange, interacting, disabled;
    private SoundManager sound;
    [SerializeField] private AudioClip interactSound;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        groundCheck = FindFirstObjectByType<GroundCheck>();
        input = FindFirstObjectByType<InputDetection>();
        arrow = transform.GetChild(0).GetComponent<SpriteRenderer>();
        symbol = arrow.transform.GetChild(0).GetComponent<SpriteRenderer>();
        sound = FindFirstObjectByType<SoundManager>();
    }

    private void Update()
    {
        DetectInputSource(); // Change to standard script reference ("InputDetection")
        SetAnimationParameters();
        SetSpritesActive();

        if (playerController == null)
        {
            SetPlayerReferences();
        }

        if (playerInRange && !disabled && !interacting && !playerController.GetUIOpen() && Input.GetButtonDown("Interact")) // Set back to false externally
        {
            IsInteracting(true);
            sound.PlaySound(interactSound);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !disabled)
        {
            if (groundCheck.isGrounded && playerController.GetMoveInput() == 0 && playerController.canMove())
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void IsInteracting(bool playerInteracting)
    {
        if (playerInteracting)
        {
            interacting = true;
        }
        else
        {
            interacting = false;
        }
    }

    public bool Interacted()
    {
        return interacting;
    }

    public void EnablePrompt(bool enable)
    {
        if (enable)
        {
            disabled = false;
        }
        else
        {
            disabled = true;
        }
    }

    private void SetAnimationParameters()
    {
        anim.SetBool("Interacting", interacting);

        if (!disabled)
        {
            anim.SetBool("WithinRange", playerInRange);
        }
        else
        {
            anim.SetBool("WithinRange", false);
        }
    }

    private void SetSpritesActive()
    {
        if (interacting || disabled)
        {
            symbol.enabled = false;
            arrow.enabled = false;
        }
        else
        {
            arrow.enabled = true;

            if (playerInRange)
            {
                symbol.enabled = true;
            }
        }
    }

    private void DetectInputSource()
    {
        if (input.GetCurrentInput() == "keyboard")
        {
            symbol.sprite = eKey;
        }
        else if (input.GetCurrentInput() == "xbox")
        {
            symbol.sprite = xButton;
        }
    }

    private void SetPlayerReferences()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        groundCheck = FindFirstObjectByType<GroundCheck>();
    }
}
