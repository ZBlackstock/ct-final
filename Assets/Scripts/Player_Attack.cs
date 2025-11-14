using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

// Player attack collisions that collide with enemy (both attacks & counters)
public class Player_Attack : MonoBehaviour
{
    private Container container;
    private CameraShake camShake;

    void Awake()
    {
        container = FindFirstObjectByType<Container>();
        camShake = FindFirstObjectByType<CameraShake>();  
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
                AttackEnemy(container.enemyHealth.TakeHitFromPlayer());
            }
        }
    }

    private void SuccessfulCounter(bool uppercut)
    {
        // Heal player & disable collisions
        container.playerHealth.AddHealth(container.playerHealth.healAmount);
        container.playerHealth.SetPlayerInvincible(1f);
        container.playerAnims.DisableAllAttackCollisions(); // May be unnecessary

        // Play effects
        container.playerAnim.SetTrigger("counterSuccess");
        container.settings.SetTimeScale(0, 0.15f, 0.02f);
        camShake.ShakeCamera(0.15f);

        // Call unique methods based on counter type
        if (uppercut)
        {
            SuccessfulUppercutCounter();
        }
        else
        {
            SuccessfulStepCounter();
        }
    }

    // Uppercut counter collision
    private void SuccessfulUppercutCounter()
    {
        container.playerController.uppercutKnockback = true;

        // Set enemy counteredState
        container.enemyHealth.Countered(true, true);

        // Snap to best looking part of animation to look good
        container.playerAnim.Play("Player_Counter_Uppercut", 0, 0.3f);

        // Play sound & particles
        container.sounds.PlaySound(container.sounds.player_UppercutCounterCollision);
        foreach (ParticleSystem p in container.particles.uppercutCounterParticles)
        {
            p.transform.localScale = new Vector3(container.playerController.faceRight ? 1 : -1, 1, 1);
        }
        container.particles.PlayParticlesFromParticleSystem(container.particles.uppercutCounterParticles);
    }

    // Step counter collision
    private void SuccessfulStepCounter()
    {
        container.playerController.stepKnockback = true;

        // Set enemy counteredState
        container.enemyHealth.Countered(true, false);

        // Snap to best looking part of animation to look good
        container.playerAnim.Play("Player_Counter_Step", 0, 0.35f);

        // Play sound & particles
        container.sounds.PlaySound(container.sounds.player_StepCounterCollision);
        container.particles.PlayParticlesFromParticleSystem(container.particles.stepCounterParticles[3]);
        container.particles.PlayParticlesFromParticleSystem(container.particles.stepCounterParticles[4]);
    }

    // Bool enemyExposed returns if enemy is vulnerable when method is called
    private void AttackEnemy(bool enemyExposed)
    {
        // Successful hit
        if (enemyExposed)
        {
            container.settings.SetTimeScale(0, 0.1f);
            camShake.ShakeCamera(0.1f);
            container.playerController.attackKnockback = true;
            container.sounds.PlaySoundRandom(container.sounds.player_AttackHits, 1, 0.95f, 1);
        }
        else // Attack countered by enemy
        {
            container.playerController.countered = true;
            container.playerAnims.DisableAllAttackCollisions();
            container.playerAnims.attack_False();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("enemy_overhead") && this.CompareTag("player_uppercut"))
        {
            container.enemyHealth.Countered(false, true);
        }
        else if (col.CompareTag("enemy_underarm") && this.CompareTag("player_step"))
        {
            container.enemyHealth.Countered(false, false);
        }
    }
}
