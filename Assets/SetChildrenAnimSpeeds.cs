using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChildrenAnimSpeeds : MonoBehaviour
{
    [SerializeField] private float animSpeed = 1;

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Animator>())
            {
                transform.GetChild(i).GetComponent<Animator>().speed = animSpeed;
            }
        }
    }

}
