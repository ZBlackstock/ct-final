using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI visualisation of player health
public class UI_Healthbar : MonoBehaviour
{
    private Player_Health playerHealth;
    private Vector2 healthbarSize;
    [SerializeField] private RectTransform healthbarTrans;
    private bool skipAnimation;
    private Animator anim;

    private void Awake()
    {
        playerHealth = FindFirstObjectByType<Player_Health>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        healthbarSize = healthbarTrans.rect.size;
    }

    private void OnEnable()
    {
        anim.SetBool("skipAnimation", skipAnimation);
    }

    private void Update()
    {
        SetHealth();

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("HUD_Appear"))
        {
            skipAnimation = true;
        }
        anim.SetBool("skipAnimation", skipAnimation);
    }

    private void SetHealth()
    {
        healthbarTrans.sizeDelta = new Vector2((playerHealth.health / playerHealth.maxHealth) * healthbarSize.x, healthbarSize.y);
    }
}
