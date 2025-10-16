using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] canvases = new GameObject[3];
    [SerializeField] private Animator fadeBlack;
    [SerializeField] private Animator[] selectedButtonAnims = new Animator[2];
    private float loadSceneTimer = 5;
    private bool visible;
    private bool select;
    [SerializeField] private GameObject controlsPanel;
    private float timer;
    [SerializeField] private AudioClip buttonSelect;
    private SoundManager sound;
    [SerializeField] private GameObject fakePlayer;

    private void Awake()
    {
        sound = FindFirstObjectByType<SoundManager>();
        Cursor.visible = false;
    }

    private void Start()
    {
        canvases[0].SetActive(true);
        canvases[1].SetActive(false);
        canvases[2].SetActive(false);

        select = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (Input.anyKeyDown && canvases[0].activeSelf)
        {
            OpenMenu();
        }

        if (visible)
        {
            loadSceneTimer -= Time.deltaTime;

            if (loadSceneTimer < 0)
            {
                loadSceneTimer = 100;
                SceneManager.LoadScene("SceneA");
            }
        }
        fadeBlack.SetBool("visible", visible);

        foreach (Animator anim in selectedButtonAnims)
        {
            anim.SetBool("select", select);
        }

        if (controlsPanel.activeSelf && timer < 0)
        {
            if (Input.anyKeyDown)
            {
                controlsPanel.SetActive(false);
            }
        }
    }

    public void OpenMenu()
    {
        PlaySound_ButtonSelect();
        print("openMenu");
        canvases[0].SetActive(false);
        canvases[1].SetActive(true);
        canvases[2].SetActive(true);
    }
    public void btn_Play()
    {
        print("play");
        PlaySound_ButtonSelect();
        fakePlayer.SetActive(false);
        visible = true;
        select = true;
    }

    public void btn_Controls()
    {
        PlaySound_ButtonSelect();
        controlsPanel.SetActive(true);
        timer = 0.1f;
    }
    public void btn_Settings()
    {
        PlaySound_ButtonSelect();
    }
    public void btn_Quit()
    {
        Application.Quit();
    }

    private void PlaySound_ButtonSelect()
    {
        print("select");
        sound.PlaySound(buttonSelect, 0.6f);
    }
}
