using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] canvases = new GameObject[3];
    [SerializeField] private Animator fadeBlack;
    private float loadSceneTimer = 5;
    private bool visible;

    private void Start()
    {
        canvases[0].SetActive(true);
        canvases[1].SetActive(false);
        canvases[2].SetActive(false);
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
    }

    public void btn_Play()
    {
        visible = true;
        canvases[2].SetActive(false); // Change to slick animation
    }

    public void OpenMenu()
    {
        canvases[0].SetActive(false);
        canvases[1].SetActive(true);
        canvases[2].SetActive(true);
    }
}
