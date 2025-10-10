using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private Animator absBack;
    [SerializeField] private Animator fadeBlack;
    private PlayerController playerController;
    private float timer = 12;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (absBack.GetBool("appear"))
        {
            timer -= Time.deltaTime;
        }

        if(timer < 3)
        {
            fadeBlack.SetBool("visible", true);
            if (timer < 0)
            {
                timer = 8;
                SceneManager.LoadScene("CutsceneA");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        absBack.SetBool("appear", true);
        playerController.SetDisableMove(true);
    }
}
