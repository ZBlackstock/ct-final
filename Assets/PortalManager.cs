using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject portal;
    private GameObject previousPortal = null;

    public void SpawnPortal(Vector2 pos, float rot, float duration)
    {
        Destroy(previousPortal);
        previousPortal = null;

        previousPortal = Instantiate(portal, pos, Quaternion.Euler(0, 0, rot), transform);
        Destroy(previousPortal, duration);
    }
}
