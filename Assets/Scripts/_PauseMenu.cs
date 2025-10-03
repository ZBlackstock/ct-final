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
    private GameObject healthbar;
    [SerializeField] private EnableOnEnter healthEnableTrigger;

    private void Awake()
    {
        postProcessing = FindFirstObjectByType<PostProcessingManager>();
        settings = FindFirstObjectByType<Settings>();
        controlsPanel = FindFirstObjectByType<UI_ControlsPanel>();
        playerController = FindFirstObjectByType<PlayerController>();
        healthbar = FindFirstObjectByType<UI_Healthbar>().gameObject;
    }

    void Start()
    {
        SetPauseMenuActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseMenuActive(!GetPauseMenuActive());
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
        transform.GetChild(0).gameObject.SetActive(active);
        transform.GetChild(1).gameObject.SetActive(active);

        postProcessing.SetVignette(active);
        postProcessing.SetDepthOfField(active ? 300 : 1);
        settings.SetTimeScale(active ? 0 : 1);
        playerController.SetUIOpen(true, active ? false : true); // Delay frame in setting UIOpen false, jumping after menu closes if spacebar pressed when menu open

        if (healthEnableTrigger.ObjectsEnabled())
        {
            healthbar.SetActive(!active);
        }
    }

    public bool GetPauseMenuActive()
    {
        return transform.GetChild(0).gameObject.activeSelf;
    }

    public void btn_Resume()
    {
        SetPauseMenuActive(false);
    }

    public void btn_MainMenu()
    {
        fadeBlackVisible = true;
        select = true;
    }

    public void btn_Controls()
    {
        controlsPanel.SetControlsPanel(true);
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
