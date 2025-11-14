using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for logic tied to enemy animations
public class Enemy_Animations : MonoBehaviour
{
    private SoundManager sound;
    private Container container;

    private void Awake()
    {
        sound = FindFirstObjectByType<SoundManager>();
        container = FindFirstObjectByType<Container>();
    }

    private void PlaySound_Step()
    {
        sound.PlaySoundRandom(sound.enemy_Steps, 0.3f, 1, 1);
    }

    private void PlayParticles_Scythe()
    {
        container.particles.PlayParticlesFromParticleSystem(container.particles.scythe);
    }

    private void SwipeSound()
    {
        container.sounds.PlaySound(container.sounds.player_AttackWhoosh[0]);
    }
}



