using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public static Container instance { get; private set; }

    [field: SerializeField] public PlayerController playerController { get; private set; }
    [field: SerializeField] public Player_Health playerHealth { get; private set; }
    [field: SerializeField] public Player_Animations playerAnims { get; private set; }
    [field: SerializeField] public Settings settings { get; private set; }
    [field: SerializeField] public SoundManager sounds { get; private set; }
    [field: SerializeField] public _ParticlesManager particles { get; private set; }
    [field: SerializeField] public Enemy_Health enemyHealth { get; private set; }
    [field: SerializeField] public Animator playerAnim{ get; private set; }


    private void Awake()
    {
        instance = this;
    }
}
