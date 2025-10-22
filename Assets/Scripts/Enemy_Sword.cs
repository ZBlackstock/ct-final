using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sword : MonoBehaviour
{
    // If this is overhead, and meets col that is uppercutcounter -> enemy_health.overheadcountered()

    private void Awake()
    {
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.CompareTag("player_uppercut") && this.CompareTag("enemy_overhead"))
        {
        }
    }


}
