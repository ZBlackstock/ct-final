using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Particles : MonoBehaviour
{
    public GameObject hurt_Particles;
    public GameObject death_Particles;
    public ParticleSystem uppercutParticles;
    public GameObject[] playerArmour_Particles;

    public void SpawnParticlesAsGameObject(GameObject particles, Vector2 pos)
    {
        Instantiate(particles, pos, Quaternion.identity);
    }

    public void SpawnParticlesAsGameObject(GameObject particles, Vector2 pos, Quaternion rotation)
    {
        Instantiate(particles, pos, rotation);
    }

    public void PlayeParticlesFromParticleSystem(ParticleSystem particles)
    {
        particles.Play();
    }
}
