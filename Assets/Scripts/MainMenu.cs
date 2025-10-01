using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Awake()
    {
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
    }

    public void btn_Play()
    {
        visible = true;
        select = true;
    }

    public void OpenMenu()
    {
        canvases[0].SetActive(false);
        canvases[1].SetActive(true);
        canvases[2].SetActive(true);
    }

    public void btn_Quit()
    {
        Application.Quit();
    }
}
