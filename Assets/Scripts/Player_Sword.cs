using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sword : MonoBehaviour
{
    private Settings settings;
    private CameraShake camShake;
    private PlayerController playerController;  

    void Awake()
    {
        settings = FindFirstObjectByType<Settings>();
        camShake = FindFirstObjectByType<CameraShake>();
        playerController = GetComponentInParent<PlayerController>();    
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("enemy"))
        {
            settings.SetTimeScale(0, 0.1f);
            camShake.ShakeCamera(0.1f);
            playerController.attackKnockback = true;    
        }
    }
}
