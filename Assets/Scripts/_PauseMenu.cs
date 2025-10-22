using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages enabling & disabling of pause menu and related canvases
public class _PauseMenu : MonoBehaviour
{
    private PostProcessingManager postProcessing; 
    private Settings settings;
    private PlayerController playerController;

    [SerializeField] private Animator[] selectedButtonAnims = new Animator[2]; // Sword highlights that indicate currently highlighted button
    [SerializeField] private Animator fadeBlack; // Fade when exiting to main menu
    private bool fadeBlackVisible; // True when exit to main menu pressed, talks to bool in fadeBlack anim
    private UI_ControlsPanel controlsPanel; // Manages controls canvas enabled/disabled. Not located on same gameobject
    private float loadSceneTimer = 3; // Wait duration before loading main menu
    private bool select; // talks to button highlighters anim bool "select"
    private GameObject pauseMenuCanvas;
    private GameObject settingsCanvas;
    [SerializeField] private GameObject controlsCanvas;
    [HideInInspector] public GameObject buttonHighlightCanvas;
    private Menu_Settings menuSettings;
    private SoundManager sound;

    // Set references
    private void Awake()
    {
        postProcessing = FindFirstObjectByType<PostProcessingManager>();
        settings = FindFirstObjectByType<Settings>();
        controlsPanel = FindFirstObjectByType<UI_ControlsPanel>();
        playerController = FindFirstObjectByType<PlayerController>();
        pauseMenuCanvas = transform.GetChild(0).gameObject;
        settingsCanvas = FindFirstObjectByType<Menu_Settings>().gameObject;
        menuSettings = settingsCanvas.GetComponent<Menu_Settings>();
        buttonHighlightCanvas = FindFirstObjectByType<ButtonSelectionHighlight>().transform.parent.gameObject;
        sound = FindFirstObjectByType<SoundManager>();
    }

    IEnumerator Start()
    {
        // Wait frame to allow gameSettings menu to execute Initialise()
        yield return null;
        SetPauseMenuActive(false);
    }

    void Update()
    {
        // Open/close pause menu
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start") && !playerController.GetDialogueOpen())
        {
            // Closes any submenus, and if any were open, returns true
            if (!Submenu_Back())
            {
                // Checks controls panel has not been opened from elsewhere
                if (!controlsCanvas.activeSelf) 
                {
                    SetPauseMenuActive(!GetPauseMenuActive());
                }
            }
        }

        // Special case for Xbox "B" - can close menu, but not open it
        if ((GetPauseMenuActive() || settingsCanvas.activeSelf || controlsCanvas.activeSelf) && Input.GetButtonDown("B"))
        {
            // Closes any submenus, and if any were open, returns true
            if (!Submenu_Back())
            {
                // Checks controls panel has not been opened from elsewhere
                if (!controlsCanvas.activeSelf)
                {
                    SetPauseMenuActive(false);
                }
            }
        }

        // Reduce timer when exit to main menu selected, load scene when hits 0
        if (fadeBlackVisible)
        {
            loadSceneTimer -= Time.unscaledDeltaTime;

            if (loadSceneTimer < 0)
            {
                LoadMainMenuScene();
            }
        }

        SetAnimBools();
    }

    // Checks if submenu like controls or settings is open on exit input, and if so wont close menu, just go back once
    private bool Submenu_Back()
    {
        if (settingsCanvas.activeSelf || controlsCanvas.activeSelf)
        {
            menuSettings.btn_Back();
            return true;
        }
        return false;
    }

    //Enables/disbales canvases, settings, and effects accordingly for enabling/disabling pause menu
    public void SetPauseMenuActive(bool active)
    {
        //Otherwise sound will play on scene start
        if(Time.timeSinceLevelLoad > 0.5f)
        {
            sound.PlaySound(sound.UI_ButtonSelect);
        }
        pauseMenuCanvas.SetActive(active);
        buttonHighlightCanvas.SetActive(active);
        settingsCanvas.SetActive(false);
        postProcessing.SetVignette(active);
        postProcessing.SetDepthOfField(active ? 300 : 1);
        settings.SetTimeScale(active ? 0 : 1);
        playerController.SetUIOpen(true, active ? false : true); // Delay frame in setting UIOpen false, jumping after menu closes if spacebar pressed when menu open
    }

    // Returns if pause menu, or any submenus are open
    public bool GetPauseMenuActive()
    {
        return pauseMenuCanvas.activeSelf || settingsCanvas.activeSelf || (controlsCanvas.activeSelf && !controlsPanel.GetControlsPanelActive());
    }

    // ----------------------Pause Menu Buttons--------------------------------------
    public void btn_Resume()
    {
        sound.PlaySound(sound.UI_ButtonSelect);
        SetPauseMenuActive(false);
    }

    public void btn_Controls()
    {
        sound.PlaySound(sound.UI_ButtonSelect);
        controlsPanel.SetControlsPanel(true);
        pauseMenuCanvas.SetActive(false);
        buttonHighlightCanvas.SetActive(false);
    }

    public void btn_Settings()
    {
        sound.PlaySound(sound.UI_ButtonSelect);
        settingsCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
    }

    public void btn_MainMenu()
    {
        sound.PlaySound(sound.UI_ButtonSelect);
        fadeBlackVisible = true;
        select = true;
    }

    private void LoadMainMenuScene()
    {
        loadSceneTimer = 100;
        settings.SetTimeScale(1);
        SceneManager.LoadScene("MainMenu");
    }

// -----------------------------------------------------------------------------------
    private void SetAnimBools()
    {
        if (fadeBlack.gameObject.activeSelf)
        {
            fadeBlack.SetBool("visible", fadeBlackVisible);
        }

        if (selectedButtonAnims[0].gameObject.activeSelf)
        {
            foreach (Animator anim in selectedButtonAnims)
            {
                anim.SetBool("select", select);
            }
        }
    }
}
