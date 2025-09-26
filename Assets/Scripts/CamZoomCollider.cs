using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoomCollider : MonoBehaviour
{
    [SerializeField] private int camDist;
    [SerializeField] private float zoomRate;
    [SerializeField] private Vector2 zoomDamping;
    private Cinemachine.CinemachineVirtualCamera cinCam;
    private static int s_colliderCount;
    public int countCopy;
    Cinemachine.CinemachineTransposer transposer;
    public bool snapCamera;
    [SerializeField] private float deadZoneHeight = 0.23f;

    void Awake()
    {
        cinCam = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
        transposer = cinCam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();

        if (zoomRate == 0)
        {
            zoomRate = 1;
        }
        if (zoomDamping.x == 0 || zoomDamping.y == 0)
        {
            zoomDamping.x = 2;
            zoomDamping.y = 2;
        }
    }

    private void Update()
    {
        if (cinCam == null)
        {
            cinCam = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
            transposer = cinCam.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();

        }
        else if (s_colliderCount < 0)
        {
            s_colliderCount = 0;
        }

        countCopy = s_colliderCount;

        transposer.m_FollowOffset.y = -transposer.m_FollowOffset.z / 7;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            s_colliderCount++;
            // StartCoroutine(GraduallyReduceSoftZone());

            if (snapCamera)
            {
                SnapCamera(camDist);
            }
            else
            {
                StartCoroutine(ZoomCamera(camDist));
            }

            //transposer.m_DeadZoneHeight = deadZoneHeight;
        }
    }

    private void SnapCamera(float newZ)
    {
        float dampingTemp = transposer.m_ZDamping;
        transposer.m_ZDamping = 0;
        transposer.m_FollowOffset.z = newZ;
        transposer.m_ZDamping = dampingTemp;
    }

    private IEnumerator ZoomCamera(float newZ)
    {
        float currentZ = transposer.m_FollowOffset.z;
        transposer.m_XDamping = zoomDamping.x;
        transposer.m_YDamping = zoomDamping.y;

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

        //transposer.m_CameraDistance = newZ;
        transposer.m_XDamping = 1;
        transposer.m_YDamping = 0.2f;
    }

    /* IEnumerator GraduallyReduceSoftZone()
     {
         transposer.m_UnlimitedSoftZone = false;
         transposer.m_SoftZoneWidth = 2;
         transposer.m_SoftZoneHeight = 2;

         while (transposer.m_SoftZoneHeight > 0.57f || transposer.m_SoftZoneWidth > 0.33f)
         {
             if (transposer.m_SoftZoneHeight > 0.57f)
             {
                 transposer.m_SoftZoneHeight -= 0.05f;
                // transposer.m_DeadZoneHeight = deadZoneHeight;

             }

             if (transposer.m_SoftZoneWidth > 0.33f)
             {
                 transposer.m_SoftZoneWidth -= 0.05f;
                 transposer.m_DeadZoneWidth = 0.02f;

             }

             yield return new WaitForSeconds(Time.deltaTime);
         }

         transposer.m_SoftZoneWidth = 0.33f;
         transposer.m_SoftZoneHeight = 0.57f;

         transposer.m_DeadZoneWidth = 0.02f;
         //transposer.m_DeadZoneHeight = deadZoneHeight;
     }
    */

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            s_colliderCount--;
        }

    }

    public void ChangeZoomValue(int value)
    {
        camDist = value;
        KickCollider();
    }

    private void KickCollider()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        box.enabled = true;
    }

    public float GetDeadZoneHeight()
    {
        return deadZoneHeight;
    }
}
