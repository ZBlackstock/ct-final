using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ParticlesManager : MonoBehaviour
{
    public GameObject hurt_Particles;
    public GameObject death_Particles;
    public ParticleSystem uppercutParticles;
    public ParticleSystem[] uppercutCounterParticles;
    public ParticleSystem[] stepCounterParticles;
    public GameObject[] playerArmour_Particles;


    [Header("Enemy Particles")]
    public ParticleSystem enemyCounter_Particles;

    public void SpawnParticlesAsGameObject(GameObject particles, Vector2 pos)
    {
        Instantiate(particles, pos, Quaternion.identity);
    }

    public void SpawnParticlesAsGameObject(GameObject particles, Vector2 pos, Quaternion rotation)
    {
        Instantiate(particles, pos, rotation);
    }

    public void PlayParticlesFromParticleSystem(ParticleSystem particles)
    {
        particles.Play();
    }

    public void PlayParticlesFromParticleSystem(ParticleSystem[] particles)
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }
}
