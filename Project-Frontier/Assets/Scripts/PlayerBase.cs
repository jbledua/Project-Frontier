using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    //This is just shared stats now!, no reason to have different enemy and player stats
    EnemyStats stats;

    private static int Iframes = 50;
    private int UpdatesSinceDmg = Iframes;
    void Start()
    {
        stats = gameObject.GetComponent<EnemyStats>();
    }
    private void FixedUpdate()
    {
        UpdatesSinceDmg++;
        if (stats.getHp() <= 0)
        {
            stats.setHp(stats.MaxHP);
            Debug.Log(stats.MaxHP);
            Debug.Log(stats.CurrentHP);
            gameObject.GetComponent<PlayerMovement>().OnPlayerKilled();
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet") && UpdatesSinceDmg >= Iframes)
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
