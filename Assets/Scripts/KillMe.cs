using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMe : MonoBehaviour
{
    [SerializeField] private float timeUntilDestroy;
    [SerializeField] private bool destroyOnTriggerEnter;

    void Start()
    {
        if (!destroyOnTriggerEnter)
        {
            Destroy(gameObject, timeUntilDestroy);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Izzy"))
        {
            if (destroyOnTriggerEnter)
            {
                Destroy(gameObject, timeUntilDestroy);
            }
        }
    }



}
