using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;
    public float waitTime;

    void Update()
    {
        waitTime -= Time.deltaTime; 

        if(waitTime < 0)
        {
            waitTime = 100;
            SceneManager.LoadScene(sceneName);
        }
    }
}
