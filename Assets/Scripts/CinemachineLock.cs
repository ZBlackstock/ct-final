using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineLock : MonoBehaviour
{

    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
