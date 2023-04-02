using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timeAlive;
    public bool isFriendly = true;

    Collider2D c;
    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<Collider2D>();
        timeAlive = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if(timeAlive > 0.25f) { Destroy(gameObject); }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (timeAlive > 0.025f && (collision.gameObject.CompareTag("Walls") || collision.gameObject.CompareTag("Ground")) )
        {
            Destroy(this.gameObject);
        }
        if (isFriendly && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyStats>().Hit(10);
        }
    }
}
