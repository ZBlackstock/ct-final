using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages player body collisisons, damage and healing
public class Player_Health : Health
{
    private float invincibilityTimer;
    [SerializeField] private float invincibilityDuration = 1.5f;
    private PlayerController playerController;
    private CameraShake cameraShake;
    [SerializeField] private Animator fadeBlackAnim;

    private bool death;
    private float sceneResetTimer = 5f;

    private float timer = -1f; // For when to freeze after taking hit
    private _ParticlesManager particlesManager;
    [SerializeField] private SpriteRenderer[] playerSprites;
    [SerializeField] private GameObject trail;
    private bool playerInvincible;
    private SoundManager sound;
    private UI_Healthbar healthbar;
    private UI_HealthbarEnemy healthbarEnemy;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        playerController = GetComponentInParent<PlayerController>();
        cameraShake = FindFirstObjectByType<CameraShake>();
        settings = FindFirstObjectByType<Settings>();
        particlesManager = GetComponentInParent<_ParticlesManager>();
        sound = FindFirstObjectByType<SoundManager>();
    }

    void Update()
    {
        invincibilityTimer -= Time.deltaTime;

        if (death)
        {
            sceneResetTimer -= Time.deltaTime;
        }

        if (sceneResetTimer < 3)
        {
            SetFadeBlackVisible();
            if (sceneResetTimer < 0)
            {
                ResetScene();
            }
        }

        if (timer < 0 && timer != -1f)
        {
            settings.SetTimeScale(0, !death ? 0.3f : 0.6f);
            timer = -1f;
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

    }

    private void TakeHit(bool fromRight)
    {
        if (invincibilityTimer < 0)
        {
            rb.velocity = Vector2.zero;

            hurt = true;
            health -= 15;
            timer = 0.05f; //Time until brief "hit pause" 

            if (health <= 0)
            {
                Player_Death();
                DeathEffects();
            }
            else
            {
                sound.PlaySound(sound.player_Hurt);
                if (fromRight && playerController.hurtKnockbackForce.x > 0 || !fromRight && playerController.hurtKnockbackForce.x < 0)
                {
                    playerController.hurtKnockbackForce = new Vector2(playerController.hurtKnockbackForce.x * -1, playerController.hurtKnockbackForce.y);
                }
                invincibilityTimer = invincibilityDuration;
                rb.gravityScale = playerController.jumpingGravity;

                HitEffects();
            }
        }
    }

    private void HitEffects()
    {
        cameraShake.ShakeCamera(0.3f);
        tintAnim.SetTrigger("hit");
        bodyAnim.SetTrigger("hurt");
        particlesManager.SpawnParticlesAsGameObject(particlesManager.hurt_Particles, bodyAnim.transform.position);
    }

    public void Player_Death()
    {
        death = true;
        invincibilityTimer = 100;
        trail.SetActive(false);
        sound.PlaySound(sound.player_Death);

        FindFirstObjectByType<UI_Healthbar>().gameObject.SetActive(false);
        FindFirstObjectByType<UI_HealthbarEnemy>().gameObject.SetActive(false);
    }

    private void DeathEffects()
    {
        cameraShake.ShakeCamera(0.6f);
        foreach (SpriteRenderer sprite in playerSprites)
        {
            sprite.enabled = false;
        }
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        particlesManager.SpawnParticlesAsGameObject(particlesManager.death_Particles, bodyAnim.transform.position);


        for (int i = 0; i < playerSprites.Length - 1; i++)
        {
            particlesManager.SpawnParticlesAsGameObject(particlesManager.playerArmour_Particles[i],
                playerSprites[i].bounds.center, playerSprites[i].transform.rotation);
        }
    }

    public bool IsDeath()
    {
        return death;
    }

    public void SetHurt(bool isHurt)
    {
        hurt = isHurt;
    }

    private IEnumerator OnTriggerStay2D(Collider2D col)
    {
        yield return null;
        if (col.tag.Contains("enemy") && invincibilityTimer < 0)
        {
            //Detect if child caused collision
            TakeHit(col.gameObject.transform.position.x > transform.position.x);
        }
    }

    public void SetFadeBlackVisible()
    {
        fadeBlackAnim.SetBool("visible", true);
    }

    private void ResetScene()
    {
        SceneManager.LoadScene("SceneA");
    }

    public void SetPlayerInvincible(float duration)
    {
        invincibilityTimer = duration;
    }

    public bool GetPlayerInvincible()
    {
        return invincibilityTimer > 0;
    }

    public void AddHealth(uint healthAdd)
    {
        if (health < maxHealth)
        {
            particlesManager.PlayParticlesFromParticleSystem(particlesManager.playerHeal);
            // Ensure health healed doesn't go over max
            health = health > (maxHealth - healthAdd) ? maxHealth : health += healthAdd;
            tintAnim.SetTrigger("heal");
        }
    }
}

