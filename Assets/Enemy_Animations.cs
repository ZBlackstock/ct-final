using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackSequence
{
    public List<GameObject> attacks;
}

public class Enemy_Animations : MonoBehaviour
{
    [SerializeField] public List<AttackSequence> attackSequences = new List<AttackSequence>();
    private SoundManager sound;
    [SerializeField] private AudioClip[] steps;

    private void Awake()
    {
        sound = FindFirstObjectByType<SoundManager>();
    }

    private void PlaySound_Step()
    {
        sound.PlaySoundRandom(steps, 0.3f, 1, 1);
    }
}



