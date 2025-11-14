using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

// Manages damage and healing of enemy entities, and effects
public class Enemy_Health : Health
{
    private Container container;

    [SerializeField] private string[] tags = new string[2]; // 2 different tags dictate if enemy is in vulnerable state
    private float invincibilityTimer;
    [SerializeField] private float invincibilityDuration = 0.15f;  // Invincibility duration after taking hit

    private CapsuleCollider2D capsule;

    private bool overheadCountered, underarmCountered; // Dictates which logic to execute when player counters enemy attack
    private bool faceRight;
    [SerializeField] private SpriteRenderer attackTrail;
    private BehaviorTree behaviourTree; // Enemy AI

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        container = FindFirstObjectByType<Container>();
        behaviourTree = GetComponentInParent<BehaviorTree>();
    }

    void Update()
    {
        invincibilityTimer -= Time.deltaTime;
        tag = container.enemy.CanMove() ? tags[0] : tags[1];
    }

    // Called when player attack collides with enemy collider
    public bool TakeHitFromPlayer()
    {
        if (invincibilityTimer < 0)
        {
            TakeHit(gameObject.CompareTag("enemy_exposed"));
        }

        return CompareTag("enemy_exposed");
    }

    // Handles hit logic
    private void TakeHit(bool exposed)
    {
        if (exposed) // take hit
        {
            health -= 8f;
            rb.velocity = Vector2.zero;
            invincibilityTimer = invincibilityDuration;

            // Communicate with enemy AI
            behaviourTree.SetVariableValue("hit", true);

            //If dead, disable collider
            capsule.enabled = health > 0;

            // Set animator variables
            tintAnim.SetTrigger("hit");
            bodyAnim.SetTrigger("hit");

            bodyAnim.SetBool("dead", health <= 0);
        }
        else // Counter player
        {
            invincibilityTimer = 0.1f;
            bodyAnim.SetTrigger("counter");
            container.particles.enemyCounter_Particles.transform.localScale = new Vector3((faceRight ? -1 : 1), 1, 1);
            container.particles.PlayParticlesFromParticleSystem(container.particles.enemyCounter_Particles);

            if (health < maxHealth)
            {
                container.particles.PlayParticlesFromParticleSystem(container.particles.enemyHeal);
                // Ensure health healed doesn't go over max
                health = health > (maxHealth - healAmount) ? maxHealth : health += healAmount;
                tintAnim.SetTrigger("heal");
            }
        }
    }

    // Set countered state based on player using uppercut/step counter
    public void Countered(bool successfullyCountered, bool overheadAttack)
    {
        if (successfullyCountered)
        {
            bodyAnim.SetTrigger(overheadAttack ? "countered_overhead" : "countered_underarm");
            attackTrail.enabled = false;
        }

        if (overheadAttack)
        {
            overheadCountered = successfullyCountered;
        }
        else
        {
            underarmCountered = successfullyCountered;
        }
    }

    // Returns if enemy is in any countered state
    public bool GetHasBeenCountered()
    {
        return overheadCountered || underarmCountered;
    }

    public void Set_FaceRight(bool right)
    {
        faceRight = right;
    }
}
