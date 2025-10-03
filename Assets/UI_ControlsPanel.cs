using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        settings = FindFirstObjectByType<Settings>();
        anim = controlsPanel.GetComponent<Animator>();
        pauseMenu = FindFirstObjectByType<_PauseMenu>();
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
            if (Input.anyKeyDown)
            {
                SetControlsPanel(false);
                interact.IsInteracting(false);
            }
        }

        if (interact.Interacted())
        {
            SetControlsPanel(true);
            interact.IsInteracting(false);
        }
    }

    public void SetControlsPanel(bool appear)
    {
        print("controls" + appear);
        playerController.SetUIOpen(appear || pauseMenu.GetPauseMenuActive(), false); // UIOPen if controls is open OR pause menu is open
        settings.SetTimeScale(appear || pauseMenu.transform.GetChild(0).gameObject.activeSelf ? 0 : 1);
        controlsPanel.SetActive(appear);
        timer = appear ? 0.1f : 0;
    }
}
