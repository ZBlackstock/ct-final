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


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



