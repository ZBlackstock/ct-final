using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        anim.SetBool("shake", timer >= 0 ? true : false);
    }

    public void ShakeCamera(float duration)
    {
        timer = duration;
    }
}
