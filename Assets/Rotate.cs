using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    private Vector3 rotation;
    private float speedUpRate;
    [SerializeField] private float maxSpeed;
    private bool speedUp;

    void Update()
    {
        rotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z + speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rotation);

        if (speedUp && Mathf.Abs(speed) < maxSpeed)
        {
            speed += speedUpRate;
        }
        else
        {
            speedUp = false;
        }
    }

    public void SpeedUp(float rate)
    {
        speedUpRate = rate;
        speedUp = true;
        Debug.Log("WORK CUNT");
    }
}
