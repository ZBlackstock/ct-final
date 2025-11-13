using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnStart : MonoBehaviour
{
    [SerializeField] private bool waitFrame;
    IEnumerator Start()
    {
        if (waitFrame)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
