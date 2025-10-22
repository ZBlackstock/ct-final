using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Zoom camera to Z distance when entering a collider
public class CamZoomCollider : MonoBehaviour
{
    [SerializeField] private int camDist;
    [SerializeField] private float zoomRate;
    [SerializeField] private Vector2 zoomDamping;
    private Cinemachine.CinemachineVirtualCamera cinCam;
    Cinemachine.CinemachineTransposer transposer;

    void Awake()
    {
        cinCam = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
        transposer = cinCam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();

        // Can't have negative or 0 zoomRate. Would be ridiculous
        if (zoomRate <= 0)
        {
            zoomRate = 1;
        }
    }

    private void Update()
    {
        if (cinCam == null)
        {
            cinCam = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
            transposer = cinCam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();
        }
        // Ensure player stays on same screen Y position regardless of zoom
        transposer.m_FollowOffset.y = -transposer.m_FollowOffset.z / 3f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ZoomCamera(-camDist));
        }
    }

    private IEnumerator ZoomCamera(float newZ)
    {
        float currentZ = transposer.m_FollowOffset.z;

        if (currentZ < newZ)
        {
            while (currentZ < newZ)
            {
                currentZ += zoomRate;
                transposer.m_FollowOffset.z = currentZ;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else if (currentZ > newZ)
        {
            while (currentZ > newZ)
            {
                currentZ -= zoomRate;
                transposer.m_FollowOffset.z = currentZ;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}
