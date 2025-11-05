using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects if gameobject is touching ground
public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;
    private CapsuleCollider2D capsule;
    [SerializeField] private LayerMask groundLayerMask;
    public float timeSinceLeftGround, boxDepth;
    [SerializeField] private Health health;
    [SerializeField] private Animator relatedAnim;

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

        if (relatedAnim != null)
        {
            relatedAnim.SetBool("isGrounded", isGrounded);
        }
    }

    public bool isOnGround(float boxDepth)
    {
        float extraHeight = boxDepth;
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsule.bounds.center, capsule.bounds.size - new Vector3(0.01f, 0f, 0f), 0f, Vector2.down, extraHeight, groundLayerMask);

        Color rayColor;
        rayColor = Color.green;

        return raycastHit.collider != null;
    }
}
