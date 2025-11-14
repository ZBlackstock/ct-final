using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by external scripts to instantiate particles as a gameobject, 
// or play existing effects
public class _ParticlesManager : MonoBehaviour
{
    public GameObject hurt_Particles; // Player hurt
    public GameObject death_Particles; // Player death (exaggerated hurt particles)
    public ParticleSystem uppercutParticles; // Dirt from ground when uppercutting
    public ParticleSystem[] uppercutCounterParticles; // Spark particles when uppercut counter is successful
    public ParticleSystem[] stepCounterParticles; // Spark particles when step counter is successful
    public GameObject[] playerArmour_Particles; // Player death (each armour piece is a particle)
    public ParticleSystem playerHeal; //Play on successful counter


    [Header("Enemy Particles")]
    public ParticleSystem enemyCounter_Particles; // Metal sparks when enemy counters player
    public ParticleSystem enemyHeal; //Play when player countered
    public ParticleSystem[] scythe;

    //Instantiate GameObject at position 
    public void SpawnParticlesAsGameObject(GameObject particles, Vector2 pos)
    {
        Instantiate(particles, pos, Quaternion.identity);
    }

    //Instantiate GameObject at position and rotation
    public void SpawnParticlesAsGameObject(GameObject particles, Vector2 pos, Quaternion rotation)
    {
        Instantiate(particles, pos, rotation);
    }

    //Play particles from ParticleSystem already in scene
    public void PlayParticlesFromParticleSystem(ParticleSystem particles)
    {
        particles.Play();
    }

    //Play multiple particles from ParticleSystem array already in scene
    public void PlayParticlesFromParticleSystem(ParticleSystem[] particles)
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }
}
