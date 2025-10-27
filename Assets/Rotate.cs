using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    private Vector3 rotation;
    void Update()
    {
        rotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 
            transform.rotation.eulerAngles.z + speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
