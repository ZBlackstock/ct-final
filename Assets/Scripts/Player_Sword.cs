using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sword : MonoBehaviour
{
    private Settings settings;
    private CameraShake camShake;
    private PlayerController playerController;
    private Player_Animations playerAnims;
    private Player_Health playerHealth;
    private Animator anim;
    Enemy_Health enemyHealth;


    void Awake()
    {
        settings = FindFirstObjectByType<Settings>();
        camShake = FindFirstObjectByType<CameraShake>();
        playerController = GetComponentInParent<PlayerController>();
        playerHealth = FindFirstObjectByType<Player_Health>();
        playerAnims = FindFirstObjectByType<Player_Animations>();
        anim = playerController.gameObject.GetComponentInChildren<Animator>();
        enemyHealth = FindFirstObjectByType<Enemy_Health>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (this.CompareTag("player_uppercut"))
        {
            if (col.CompareTag("enemy_overhead"))
            {
                anim.Play("Player_Counter_Uppercut", 0, 0.35f);
                anim.SetTrigger("counterSuccess");
                settings.SetTimeScale(0, 0.1f, 0.1f);
                camShake.ShakeCamera(0.1f);
                playerAnims.DisableAllAttackCollisions();
                playerAnims.uppercut_False();
                playerController.attackKnockback = true;
                playerHealth.SetPlayerInvincible(1f);
                enemyHealth.OverheadCountered(true);
                playerController.uppercutKnockback = true;
            }
        }
        else if (this.CompareTag("player_step"))
        {

        }
        else // Regular attack
        {
            if (col.CompareTag("enemy_exposed"))
            {
                settings.SetTimeScale(0, 0.1f);
                camShake.ShakeCamera(0.1f);
                playerController.attackKnockback = true;
            }
            else if (col.CompareTag("enemy_unexposed"))
            {
                settings.SetTimeScale(0, 0.15f);
                playerController.countered = true;
                playerAnims.DisableAllAttackCollisions();
                playerAnims.attack_False();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.CompareTag("enemy_overhead") && this.CompareTag("player_uppercut"))
        {
            enemyHealth.OverheadCountered(false);
        }
    }
}
