using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets object rotation to another gameobjects rotation
public class AlignRotationWithObject : MonoBehaviour
{
    [SerializeField] private Transform obj;
    [SerializeField] private float z_multiplier = 1;

    void Update()
    {
        if(obj != null)
        {
            Vector3 objEulerRotation = new Vector3(obj.transform.eulerAngles.x, 
                obj.transform.eulerAngles.y, obj.transform.eulerAngles.z * z_multiplier);

            transform.rotation = Quaternion.Euler(objEulerRotation);  
        }
    }
}
