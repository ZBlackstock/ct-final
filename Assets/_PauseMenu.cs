using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _PauseMenu : MonoBehaviour
{
    private PostProcessingManager postProcessing;
    private Settings settings;

    [SerializeField] private Animator[] selectedButtonAnims = new Animator[2];
    [SerializeField] private Animator fadeBlack;
    private float loadSceneTimer = 3;
    private bool visible;
    private bool select;

    private void Awake()
    {
        postProcessing = FindFirstObjectByType<PostProcessingManager>();
        settings = FindFirstObjectByType<Settings>();
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

        if (visible)
        {
            loadSceneTimer -= Time.unscaledDeltaTime;

            if (loadSceneTimer < 0)
            {
                loadSceneTimer = 100;
                settings.SetTimeScale(1);
                SceneManager.LoadScene("MainMenu");
            }
        }
        fadeBlack.SetBool("visible", visible);

        foreach (Animator anim in selectedButtonAnims)
        {
            anim.SetBool("select", select);
        }
    }

    public void SetPauseMenuActive(bool active)
    {
        transform.GetChild(0).gameObject.SetActive(active);
        transform.GetChild(1).gameObject.SetActive(active);

        if (active)
        {
            settings.SetTimeScale(0);

            postProcessing.SetDepthOfField(300);
            postProcessing.SetVignette(true);
        }
        else
        {
            settings.SetTimeScale(1);

            postProcessing.SetDepthOfField(1);
            postProcessing.SetVignette(false);
        }
    }

    public bool GetPauseMenuActive()
    {
        return transform.GetChild(0).gameObject.activeSelf;
    }

    public void btn_MainMenu()
    {
        visible = true;
        select = true;
    }
}
