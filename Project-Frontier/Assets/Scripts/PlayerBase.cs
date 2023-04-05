using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    //This is just shared stats now!, no reason to have different enemy and player stats
    EnemyStats stats;
    void Start()
    {
        stats = gameObject.GetComponent<EnemyStats>();
    }
    private void FixedUpdate()
    {
        if (stats.getHp() <= 0)
        {
            gameObject.GetComponent<PlayerMovement>().OnPlayerKilled();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Bullet b = collision.collider.GetComponent<Bullet>();
            if (!b.isPlayers) { stats.Hit(b.dmg); }
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            stats.Hit(collision.collider.GetComponent<EnemyStats>().OnImpactDMG);
        }
    }
}
