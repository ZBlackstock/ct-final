using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class Enemy_Health : Health
{
    [SerializeField] private string[] tags = new string[2];
    private float invincibilityTimer;
    [SerializeField] private float invincibilityDuration = 0.15f;

    private GravetenderKnight gravetenderKnight;
    private CapsuleCollider2D capsule;

    private bool dead;
    private bool counter;
    private bool overheadCountered, underarmCountered;
    private _ParticlesManager particlesManager;
    private bool faceRight;
    [SerializeField] private SpriteRenderer attackTrail;
    private BehaviorTree behaviourTree;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        gravetenderKnight = GetComponentInParent<GravetenderKnight>();
        particlesManager = FindFirstObjectByType<_ParticlesManager>();
        behaviourTree = GetComponentInParent<BehaviorTree>();
    }

    void Update()
    {
        invincibilityTimer -= Time.deltaTime;
        tag = gravetenderKnight.CanMove() ? tags[0] : tags[1];
    }

    public bool TakeHitFromPlayer()
    {
        if (invincibilityTimer < 0)
        {
            TakeHit(gameObject.CompareTag("enemy_exposed"));
        }

        return CompareTag("enemy_exposed");
    }

    private void TakeHit(bool exposed)
    {
        if (exposed) // take hit
        {
            health -= 8f;
            rb.velocity = Vector2.zero;
            invincibilityTimer = invincibilityDuration;
            tintAnim.SetTrigger("hit");
            bodyAnim.SetTrigger("hit");
            bodyAnim.SetBool("dead", health <= 0);
            capsule.enabled = health > 0;
            behaviourTree.SetVariableValue("hit", true);
        }
        else // Counter player
        {
            invincibilityTimer = 0.1f;
            counter = true;
            bodyAnim.SetTrigger("counter");
            particlesManager.enemyCounter_Particles.transform.localScale = new Vector3((faceRight ? -1 : 1), 1, 1);
            particlesManager.PlayParticlesFromParticleSystem(particlesManager.enemyCounter_Particles);

            if(health < maxHealth)
            {
                particlesManager.PlayParticlesFromParticleSystem(particlesManager.enemyHeal);
                // Ensure health healed doesn't go over max
                health = health > (maxHealth - healAmount) ? maxHealth : health += healAmount;
                tintAnim.SetTrigger("heal");
            }
        }
    }

    public void OverheadCountered(bool countered)
    {
        if (countered)
        {
            bodyAnim.SetTrigger("countered_overhead");
            attackTrail.enabled = false;
        }

        overheadCountered = countered;
    }

    public void UnderarmCountered(bool countered)
    {
        if (countered)
        {
            bodyAnim.SetTrigger("countered_underarm");
            attackTrail.enabled = false;
        }

        underarmCountered = countered;
    }

    public bool GetHasBeenCountered()
    {
        return overheadCountered || underarmCountered;
    }

    public void Set_FaceRight(bool right)
    {
        faceRight = right;
    }
}
