using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Player_Sword : MonoBehaviour
{
    private Settings settings;
    private CameraShake camShake;
    private PlayerController playerController;
    private Player_Animations playerAnims;
    private Player_Health playerHealth;
    private Animator anim;
    private Enemy_Health enemyHealth;
    private _ParticlesManager particlesManager;
    private SoundManager sound;

    void Awake()
    {
        settings = FindFirstObjectByType<Settings>();
        camShake = FindFirstObjectByType<CameraShake>();
        playerController = GetComponentInParent<PlayerController>();
        playerHealth = FindFirstObjectByType<Player_Health>();
        playerAnims = FindFirstObjectByType<Player_Animations>();
        anim = playerController.gameObject.GetComponentInChildren<Animator>();
        enemyHealth = FindFirstObjectByType<Enemy_Health>();
        particlesManager = FindFirstObjectByType<_ParticlesManager>();
        sound = FindFirstObjectByType<SoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Contains("enemy"))
        {
            // Uppercut Counter
            if (this.CompareTag("player_uppercut"))
            {
                if (col.CompareTag("enemy_overhead"))
                {
                    SuccessfulCounter(true);
                }
            } // Step Counter
            else if (this.CompareTag("player_step"))
            {
                if (col.CompareTag("enemy_underarm"))
                {
                    SuccessfulCounter(false);
                }
            }
            else // Regular attack
            {
                AttackEnemy(enemyHealth.TakeHitFromPlayer());
            }
        }
    }


    private void SuccessfulCounter(bool uppercut)
    {
        playerHealth.AddHealth(playerHealth.healAmount);
        playerHealth.SetPlayerInvincible(1f);

        anim.SetTrigger("counterSuccess");
        settings.SetTimeScale(0, 0.15f, 0.01f);
        camShake.ShakeCamera(0.15f);
        playerAnims.DisableAllAttackCollisions();

        if (uppercut)
        {
            SuccessfulUppercutCounter();
        }
        else // Step counter success
        {
            SuccessfulStepCounter();
        }
    }

    private void SuccessfulUppercutCounter()
    {
        playerController.uppercutKnockback = true;
        sound.PlaySound(sound.player_UppercutCounterCollision);
        anim.Play("Player_Counter_Uppercut", 0, 0.3f);
        playerAnims.uppercut_False();
        enemyHealth.OverheadCountered(true);

        foreach (ParticleSystem p in particlesManager.uppercutCounterParticles)
        {
            p.transform.localScale = new Vector3(playerController.faceRight ? 1 : -1, 1, 1);
        }
        particlesManager.PlayParticlesFromParticleSystem(particlesManager.uppercutCounterParticles);
    }

    private void SuccessfulStepCounter()
    {
        playerController.stepKnockback = true;
        sound.PlaySound(sound.player_StepCounterCollision);
        anim.Play("Player_Counter_Step", 0, 0.35f);
        playerAnims.step_False();
        enemyHealth.UnderarmCountered(true);
        particlesManager.PlayParticlesFromParticleSystem(particlesManager.stepCounterParticles);
    }

    private void AttackEnemy(bool enemyExposed)
    {
        if (enemyExposed)
        {
            settings.SetTimeScale(0, 0.1f);
            camShake.ShakeCamera(0.1f);
            playerController.attackKnockback = true;
            sound.PlaySoundRandom(sound.player_AttackHits, 1, 0.95f, 1);
        }
        else // Attack countered by enemy
        {
            playerController.countered = true;
            playerAnims.DisableAllAttackCollisions();
            playerAnims.attack_False();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.CompareTag("enemy_overhead") && this.CompareTag("player_uppercut"))
        {
            enemyHealth.OverheadCountered(false);
        }
        else if (col.CompareTag("enemy_underarm") && this.CompareTag("player_step"))
        {
            enemyHealth.UnderarmCountered(false);
        }
    }
}
