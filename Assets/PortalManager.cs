using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject portal;

    public void SpawnPortal(Vector2 pos, float rot, float duration)
    {
       GameObject obj = Instantiate(portal, pos, Quaternion.Euler(0, 0, rot), transform);
        Destroy(obj, duration);
    }
}
