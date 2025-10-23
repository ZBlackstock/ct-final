using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

// BAsic global settings, just timeScale currently
public class Settings : MonoBehaviour
{
    [SerializeField] private float timeScale;
    private _PauseMenu pauseMenu;
    private float timer;

    private void Awake()
    {
        pauseMenu = FindFirstObjectByType<_PauseMenu>();
    }

    void Update()
    {
        Application.targetFrameRate = 60;

        if (!pauseMenu.GetPauseMenuActive())
        {
            if (timer < 0)
            {
                Time.timeScale = timeScale;
                timer = -1f;
            }
            else
            {
                // Reduce timer only when temporary new timescale has been set
                timer -= Time.unscaledDeltaTime;
            }
        }
    }
   
    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void SetTimeScale(float time, float duration)
    {
        Time.timeScale = time;
        timer = duration;
    }

    public void SetTimeScale(float time, float duration, float executeAfter)
    {
        Time.timeScale = time;
        timer = duration;
    }
}
