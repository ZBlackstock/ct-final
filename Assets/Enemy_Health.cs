using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [SerializeField] private string[] tags = new string[2];
    private float invincibilityTimer;
    [SerializeField] private float invincibilityDuration = 0.15f;
    private Rigidbody2D rb;
    [SerializeField] private Animator tintAnim;
    [SerializeField] private Animator bodyAnim;
    private GravetenderKnight gravetenderKnight;
    private CapsuleCollider2D capsule;
    public float maxHealth = 100;
    public float health = 100;
    private bool dead;
    private bool counter;
    private bool overheadCountered;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
        gravetenderKnight = GetComponentInParent<GravetenderKnight>();
    }

    void Update()
    {
        invincibilityTimer -= Time.deltaTime;
        tag = gravetenderKnight.CanMove() ? tags[0] : tags[1];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("playerAttack") && invincibilityTimer < 0)
        {
            TakeHit(gameObject.CompareTag("enemy_exposed"));
        }
    }

    private void TakeHit(bool exposed)
    {
        if (exposed)
        {
            health -= 8f;
            rb.velocity = Vector2.zero;
            invincibilityTimer = invincibilityDuration;
            tintAnim.SetTrigger("hit");
            bodyAnim.SetTrigger("hit");
            bodyAnim.SetBool("dead", health <= 0);
            capsule.enabled = health > 0;
            
        }
        else
        {
            counter = true;
            bodyAnim.SetTrigger("counter");
        }
    }

    public void OverheadCountered(bool countered)
    {
        if (countered)
        {
            bodyAnim.SetTrigger("countered_overhead");
        }

        overheadCountered = countered;
    }

    public bool GetHasBeenCountered()
    {
        return overheadCountered;
    }
}
