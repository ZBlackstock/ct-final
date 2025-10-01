using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;
    private CapsuleCollider2D capsule;
    [SerializeField] private LayerMask groundLayerMask;
    public float timeSinceLeftGround, boxDepth;
    [SerializeField] private Player_Health health;

    private void Awake()
    {
        capsule = transform.parent.GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (isOnGround(boxDepth) && !health.IsHurt())
        {
            isGrounded = true;
            timeSinceLeftGround = 0.15f;
        }
        else
        {
            boxDepth = 0.05f; // Decrease depth when not on ground
            isGrounded = false;

            timeSinceLeftGround -= Time.deltaTime;
        }
    }

    public bool isOnGround(float boxDepth)
    {
        float extraHeight = boxDepth;
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsule.bounds.center, capsule.bounds.size - new Vector3(0.01f, 0f, 0f), 0f, Vector2.down, extraHeight, groundLayerMask);

        Color rayColor;
        rayColor = Color.green;
        Debug.DrawRay(capsule.bounds.center + new Vector3(capsule.bounds.extents.x, 0), Vector2.down * (capsule.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(capsule.bounds.center - new Vector3(capsule.bounds.extents.x, 0), Vector2.down * (capsule.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(capsule.bounds.center - new Vector3(capsule.bounds.extents.x, capsule.bounds.extents.y + extraHeight), Vector2.right * (capsule.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;
    }
}
