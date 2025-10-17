using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_ControlsPanel : MonoBehaviour
{
    [SerializeField] private GameObject controlsPanel;
    private _PauseMenu pauseMenu;
    [SerializeField] private InteractPrompt interact;
    private PlayerController playerController;
    private Settings settings;
    private Animator anim;
    private float timer = 0.1f;
    private GameObject healthbar;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        settings = FindFirstObjectByType<Settings>();
        anim = controlsPanel.GetComponent<Animator>();
        pauseMenu = FindFirstObjectByType<_PauseMenu>();
        healthbar = FindFirstObjectByType<UI_Healthbar>().gameObject;
    }

    private void Start()
    {
        controlsPanel.SetActive(false);
    }

    void Update()
    {
        timer -= Time.unscaledDeltaTime;

        if (controlsPanel.activeSelf && timer < 0)
        {
            if (!pauseMenu.GetPauseMenuActive())
            {
                if (Input.anyKeyDown)
                {
                    SetControlsPanel(false);
                    if (interact != null)
                    {
                        interact.IsInteracting(false);
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start") || Input.GetButton("B"))
                {
                    SetControlsPanel(false);
                    if (interact != null)
                    {
                        interact.IsInteracting(false);
                    }
                }
            }
        }

        if (interact != null)
        {
            if (interact.Interacted())
            {
                SetControlsPanel(true);
                interact.IsInteracting(false);
            }
        }

    }

    public void SetControlsPanel(bool appear)
    {
        if (!pauseMenu.GetPauseMenuActive())
        {
            healthbar.SetActive(!appear);
        }
        playerController.SetUIOpen(appear || pauseMenu.GetPauseMenuActive(), false); // UIOPen if controls is open OR pause menu is open1
        controlsPanel.SetActive(appear);
        pauseMenu.buttonHighlightCanvas.SetActive(pauseMenu.GetPauseMenuActive() && !appear);
        timer = appear ? 0.1f : 0;
    }
}
