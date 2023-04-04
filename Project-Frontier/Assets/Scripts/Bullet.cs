using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullet class, bullets will delete themselves after a certain amount of time or on collision
//Add animations or whatever!
//would be really easy to set a delay to the destroy command and run an animations
//or even better create a onBulletHit prefab and spawn that on collision

public class Bullet : MonoBehaviour
{
    private float timeAlive;
    public bool isPlayers = false;

    public int dmg = 5;

    // Start is called before the first frame update
    void Start()
    {
        timeAlive = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > 0.25f) { Destroy(gameObject); }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (timeAlive > 0.025f && (collision.gameObject.CompareTag("Walls") || collision.gameObject.CompareTag("Ground")))
        {
            Destroy(this.gameObject);
        }
        if (isPlayers && collision.gameObject.CompareTag("Enemy"))
        {
            //I like this method for interacting with other scripts probably a smarter/less hacky way to do this.
            collision.gameObject.GetComponent<EnemyStats>().Hit(10);
            Destroy(this.gameObject);
        }
        if (!isPlayers && collision.gameObject.CompareTag("Player"))
        {
            //however player hp is delt with can happen here
            Destroy(this.gameObject);
        }
    }
    public int getDmg() { return dmg; } 
}
