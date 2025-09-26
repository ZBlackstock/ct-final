using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player_Health : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private bool hurt;
    private float invincibilityTimer;
    [SerializeField] private float invincibilityDuration = 1.5f;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private CameraShake cameraShake;
    [SerializeField] private Animator fadeBlackAnim;

    public int health = 10;
    private bool death;
    private float sceneResetTimer = 5f;
    [SerializeField] private Animator tintAnim;
    private Settings settings;

    private float timer = -1f; // For when to freeze after taking hit

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        playerController = GetComponentInParent<PlayerController>();
        cameraShake = FindFirstObjectByType<CameraShake>();
        settings = FindFirstObjectByType<Settings>();
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
            settings.SetTimeScale(0, 0.3f);
            timer = -1f;
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

    }

    private void TakeHit(bool fromRight)
    {
        hurt = true;
        health--;

        invincibilityTimer = invincibilityDuration;
        rb.gravityScale = playerController.jumpingGravity;
        if (fromRight && playerController.hurtKnockbackForce.x > 0 || !fromRight && playerController.hurtKnockbackForce.x < 0)
        {
            playerController.hurtKnockbackForce = new Vector2(playerController.hurtKnockbackForce.x * -1, playerController.hurtKnockbackForce.y);
        }

        cameraShake.ShakeCamera(0.3f);
        tintAnim.SetTrigger("hit");
        anim.SetBool("hurt", true);
        timer = 0.1f;

        if (health <= 0)
        {
            Player_Death();
        }
    }

    public bool IsHurt()
    {
        return hurt;
    }

    public void SetHurt(bool isHurt)
    {
        hurt = isHurt;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("enemy") && invincibilityTimer < 0)
        {
            //Detect if child caused collision
            TakeHit(col.gameObject.transform.position.x > transform.position.x);
            rb.velocity = Vector2.zero;
        }
    }

    public void Player_Death()
    {
        death = true;
    }

    public void SetFadeBlackVisible()
    {
        fadeBlackAnim.SetBool("visible", true);
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

