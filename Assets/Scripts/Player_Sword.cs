using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sword : MonoBehaviour
{
    private Settings settings;
    private CameraShake camShake;
    private PlayerController playerController;
    private Player_Animations playerAnims;

    void Awake()
    {
        settings = FindFirstObjectByType<Settings>();
        camShake = FindFirstObjectByType<CameraShake>();
        playerController = GetComponentInParent<PlayerController>();
        playerAnims = FindFirstObjectByType<Player_Animations>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("enemy"))
        {
            settings.SetTimeScale(0, 0.1f);
            camShake.ShakeCamera(0.1f);
            playerController.attackKnockback = true;
        }
        else if (col.CompareTag("enemy_unexposed"))
        {
            camShake.ShakeCamera(0.05f);
            playerController.countered = true;
            playerAnims.DisableAllAttackCollisions();
            playerAnims.attack_False();
        }
    }
}
