using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shakes the camera via animation. Called externally
public class CameraShake : MonoBehaviour
{
    private float timer;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        timer -= Time.deltaTime;

        // If timer is above 0, shake camera
        anim.SetBool("shake", timer >= 0 ? true : false);
    }

    public void ShakeCamera(float duration)
    {
        timer = duration;
    }
}
