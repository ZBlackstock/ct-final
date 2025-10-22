using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animations : MonoBehaviour
{
    private SoundManager sound;

    private void Awake()
    {
        sound = FindFirstObjectByType<SoundManager>();
    }

    private void PlaySound_Step()
    {
        sound.PlaySoundRandom(sound.enemy_Steps, 0.3f, 1, 1);
    }
}



