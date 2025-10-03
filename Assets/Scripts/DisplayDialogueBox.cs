using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDialogueBox : MonoBehaviour
{
    [SerializeField] private InteractPrompt prompt;
    [SerializeField] private GameObject dialogueBox;
    private PlayerController playerController;
    [SerializeField] private Transform spawnTrans;
    private bool invokingInteractPromptEnable;


    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();

    }

    void Update()
    {
        if (prompt.interacting && !playerController.GetUIOpen()) // Integrate move.gameMenuIsActive Into Prompt
        {
            EnableDialogueBox();
        }

        if ((Input.GetButtonDown("Cancel") || Input.GetButtonDown("Joystick1")) && dialogueBox.activeSelf)
        {
            DisableDialogueBox();
        }

        if (!dialogueBox.activeSelf && prompt.disabled)
        {
            if (!invokingInteractPromptEnable)
            {
                invokingInteractPromptEnable = true;
                Invoke(nameof(EnablePrompt), 0.5f);
            }
        }
    }

    private void EnableDialogueBox()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }

        playerController.SetUIOpen(true, false);
        Vector3 spawn = spawnTrans.position;

        prompt.IsInteracting(false);
        prompt.EnablePrompt(false);

        dialogueBox.SetActive(true);
        dialogueBox.GetComponent<DialogueBox>().StartDialogue();
        dialogueBox.GetComponent<DialogueBox>().spawnTrans = spawn;
    }

    public void DisableDialogueBox()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }

        prompt.EnablePrompt(true);
        StartCoroutine(dialogueBox.GetComponent<DialogueBox>().DisableDialogueBox());
    }

    private void EnablePrompt()
    {
        prompt.EnablePrompt(true);
        invokingInteractPromptEnable = false;
    }
}
