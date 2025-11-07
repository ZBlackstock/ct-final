using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Manages enabling/disabling of controls panel, in gameplay & menus
public class UI_ControlsPanel : MonoBehaviour
{
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private InteractPrompt interact;
    private _PauseMenu pauseMenu;
    private PlayerController playerController;
    private float timer = 0.1f;
    private bool panelActive;
    private SoundManager sound;
    [SerializeField] private GameObject playerHealthbar;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        pauseMenu = FindFirstObjectByType<_PauseMenu>();
        sound = FindFirstObjectByType<SoundManager>();
    }

    private void Start()
    {
        controlsPanel.SetActive(false);
    }

    void Update()
    {
        timer -= Time.unscaledDeltaTime;

        // Check player wants to exit controls
        if (controlsPanel.activeSelf && timer < 0)
        {
            ExitCheck(pauseMenu.GetPauseMenuActive());
        }

        // Check player is interacting with prompt
        if (interact != null)
        {
            InteractCheck();
        }
    }

    private void ExitCheck(bool pauseMenuActive)
    {
        if (pauseMenuActive)
        {
            // Within pause menu, check for "back" inputs
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start") || Input.GetButton("B"))
            {
                SetControlsPanel(false);
                panelActive = false;
                if (interact != null)
                {
                    interact.IsInteracting(false);
                }
            }
        }
        else
        {
            // Not in pause menu, check for any key
            if (Input.anyKeyDown)
            {
                SetControlsPanel(false);
                panelActive = false;
                if (interact != null)
                {
                    interact.IsInteracting(false);
                }
            }
        }
    }

    // Checks if associated interact prompt has been interacted with
    private void InteractCheck()
    {
        if (interact.Interacted())
        {
            SetControlsPanel(true);
            sound.PlaySound(sound.UI_ControlsPanelAppear);
            interact.IsInteracting(false);
            panelActive = true;
        }
    }

    // Set panel active/inactive
    public void SetControlsPanel(bool appear)
    {
        if (!pauseMenu.GetPauseMenuActive())
        {
            playerHealthbar.SetActive(!appear);
        }
        playerController.SetUIOpen(appear || pauseMenu.GetPauseMenuActive(), false); // UIOPen if controls is open OR pause menu is open1
        controlsPanel.SetActive(appear);
        pauseMenu.buttonHighlightCanvas.SetActive(pauseMenu.GetPauseMenuActive() && !appear);
        timer = appear ? 0.1f : 0;
    }

    public bool GetControlsPanelActive()
    {
        return panelActive;
    }
}
