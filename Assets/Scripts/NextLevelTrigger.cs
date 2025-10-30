using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Load scene upon player entering trigger
public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private Animator absBack;
    [SerializeField] private Animator fadeBlack;
    private PlayerController playerController;
    [SerializeField] private string sceneToLoad = "CutsceneA";
    [SerializeField] private float waitDuration = 8;
    private float timer = 12;
    private bool appear;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (appear)
        {
            timer -= Time.deltaTime;
        }

        if(timer < 3)
        {
            fadeBlack.SetBool("visible", true);
            if (timer < 0)
            {
                timer = waitDuration;
                SceneManager.LoadScene(sceneToLoad);
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
        appear = true;
        if(absBack != null)
        {
            absBack.SetBool("appear", true);
        }
        playerController.SetDisableMove(true);
    }
}
