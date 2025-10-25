using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enables gameObject upon entering a trigger
public class EnableOnEnter : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToEnable;
    private bool objectsEnabled;

    private void Awake()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
                objectsEnabled = true;
            }
        }
    }

    public bool ObjectsEnabled()
    {
        return objectsEnabled;
    }
}
