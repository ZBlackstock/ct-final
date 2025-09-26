using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private float timeScale;
    private float timer;

    void Update()
    {
        Application.targetFrameRate = 120;

        if(timer < 0)
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
