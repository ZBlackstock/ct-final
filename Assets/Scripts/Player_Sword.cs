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
    private _ParticlesManager particlesManager;
    private SoundManager sound;

    [SerializeField] private AudioClip uppercutCounterCollision, stepCounterCollision;
    [SerializeField] private AudioClip[] hits;

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
        // Uppercut Counter
        if (this.CompareTag("player_uppercut"))
        {
            if (col.CompareTag("enemy_overhead"))
            {
                SuccessfulUppercutCounter();
            }
        } // Step Counter
        else if (this.CompareTag("player_step"))
        {
            if (col.CompareTag("enemy_underarm"))
            {
                SuccessfulStepCounter();
            }
        }
        else // Regular attack
        {
            if (col.CompareTag("enemy_exposed"))
            {
                AttackLand();
            }
            else if (col.CompareTag("enemy_unexposed"))
            {
                AttackCountered();
            }
        }
    }

    private void SuccessfulUppercutCounter()
    {
        playerController.uppercutKnockback = true;
        sound.PlaySound(uppercutCounterCollision);
        anim.Play("Player_Counter_Uppercut", 0, 0.3f);
        anim.SetTrigger("counterSuccess");
        settings.SetTimeScale(0, 0.15f, 0.01f);
        camShake.ShakeCamera(0.15f);
        playerAnims.DisableAllAttackCollisions();
        playerAnims.uppercut_False();
        playerHealth.SetPlayerInvincible(1f);
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
        sound.PlaySound(stepCounterCollision);
        anim.Play("Player_Counter_Step", 0, 0.35f);
        anim.SetTrigger("counterSuccess");
        settings.SetTimeScale(0, 0.1f, 0.1f);
        camShake.ShakeCamera(0.1f);
        playerAnims.DisableAllAttackCollisions();
        playerAnims.step_False();
        playerHealth.SetPlayerInvincible(1f);
        enemyHealth.UnderarmCountered(true);

        particlesManager.PlayParticlesFromParticleSystem(particlesManager.stepCounterParticles);
    }

    private void AttackLand()
    {
        settings.SetTimeScale(0, 0.1f);
        camShake.ShakeCamera(0.1f);
        playerController.attackKnockback = true;
        sound.PlaySoundRandom(hits, 1, 0.95f, 1);
    }

    private void AttackCountered()
    {
        playerController.countered = true;
        playerAnims.DisableAllAttackCollisions();
        playerAnims.attack_False();
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
