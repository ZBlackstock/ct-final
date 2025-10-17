using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _PauseMenu : MonoBehaviour
{
    private PostProcessingManager postProcessing;
    private Settings settings;
    private PlayerController playerController;

    [SerializeField] private Animator[] selectedButtonAnims = new Animator[2];
    [SerializeField] private Animator fadeBlack;
    private UI_ControlsPanel controlsPanel;
    private float loadSceneTimer = 3;
    private bool fadeBlackVisible;
    private bool select;
    private GameObject pauseMenuCanvas;
    private GameObject settingsCanvas;
    [SerializeField] private GameObject controlsCanvas;
    [HideInInspector] public GameObject buttonHighlightCanvas;
    private Menu_Settings menuSettings;
    private SoundManager sound;
    [SerializeField] private AudioClip buttonSelect;

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
        // Wait frame to allow pause menu to execute Initialise()
        yield return null;
        SetPauseMenuActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start") && !playerController.GetDialogueOpen())
        {
            if (settingsCanvas.activeSelf || controlsCanvas.activeSelf)
            {
                menuSettings.btn_Back();
            }
            else if(!controlsCanvas.activeSelf)
            {
                SetPauseMenuActive(!GetPauseMenuActive());
            }
        }
        if ((GetPauseMenuActive() || settingsCanvas.activeSelf || controlsPanel.gameObject.activeSelf) && Input.GetButtonDown("B"))
        {
            if (settingsCanvas.activeSelf || controlsCanvas.activeSelf)
            {
                menuSettings.btn_Back();
            }
            else if(!controlsCanvas.activeSelf)
            {
                SetPauseMenuActive(false);
            }
        }

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

    public void SetPauseMenuActive(bool active)
    {
        pauseMenuCanvas.SetActive(active);
        buttonHighlightCanvas.SetActive(active);
        settingsCanvas.SetActive(false);
        postProcessing.SetVignette(active);
        postProcessing.SetDepthOfField(active ? 300 : 1);
        settings.SetTimeScale(active ? 0 : 1);
        playerController.SetUIOpen(true, active ? false : true); // Delay frame in setting UIOpen false, jumping after menu closes if spacebar pressed when menu open
    }

    public bool GetPauseMenuActive()
    {
        return pauseMenuCanvas.activeSelf || settingsCanvas.activeSelf || controlsCanvas.activeSelf;
    }

    public void btn_Resume()
    {
        sound.PlaySound(buttonSelect);
        SetPauseMenuActive(false);
    }

    public void btn_Controls()
    {
        sound.PlaySound(buttonSelect);
        controlsPanel.SetControlsPanel(true);
        pauseMenuCanvas.SetActive(false);
        buttonHighlightCanvas.SetActive(false);
    }

    public void btn_Settings()
    {
        sound.PlaySound(buttonSelect);
        settingsCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
    }

    public void btn_MainMenu()
    {
        sound.PlaySound(buttonSelect);
        fadeBlackVisible = true;
        select = true;
    }

    private void LoadMainMenuScene()
    {
        loadSceneTimer = 100;
        settings.SetTimeScale(1);
        SceneManager.LoadScene("MainMenu");
    }

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
