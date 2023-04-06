using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//This is a base class for an enemy sprite
//Will detect damage, and move towards player mindlessly
//Preferably attacks should be held in seperate class
//or better yet passed as first class functions to some sort of attack manager! but thats too fancy
public class EnemyBase : MonoBehaviour
{
    Animator ani;

    EnemyStats stats;
    GameObject player;
    SpriteRenderer spRdr;
    //Movement
    private float timeSinceLastMove = 0;
    Vector3 movingTo = Vector3.zero;
    Vector3 movingFrom = Vector3.zero;
    public float speed = 0.5f;
    public float jumpTime = 10f;
    public bool isJumping = false;
    public bool isGrounded = false;
    private float timeInJump = 0f;


    private static int Iframes = 25; 
    private int UpdatesSinceDmg = Iframes;
    private float elaspedtime = 0f;
    public Quaternion initialRot = Quaternion.identity;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ani = GetComponent<Animator>();
        spRdr = GetComponent<SpriteRenderer>();
        stats = gameObject.GetComponent<EnemyStats>();
        stats.setHp(10);
        stats.setDefense(2);
        stats.setMaxHP(10);
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 15) //Sprites will only agro if within 15 units
        {
            moveToPlayer();
        }
        else
        {
            movingFrom = Vector3.zero;
            movingTo = Vector3.zero;
        }
        if (UpdatesSinceDmg < Iframes) UpdatesSinceDmg++;
        if(stats.getHp() <= 0 && Quaternion.identity == initialRot)
        {
            initialRot = gameObject.transform.rotation;
            deathAni();
            ani.SetTrigger("Death");
            return;
        }
        if(initialRot != Quaternion.identity) {
            if (deathAni()) { Destroy(gameObject); }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && UpdatesSinceDmg >= Iframes){
            UpdatesSinceDmg = 0;
            stats.Hit(collision.gameObject.GetComponent<Bullet>().getDmg());
            ani.SetTrigger("Damage");
        }
        if (collision.gameObject.tag == "Ground") { isGrounded = true; }
        if (collision.gameObject.tag == "Platform") { isGrounded = true; }
    }
    private bool deathAni()
    {
        if (gameObject.transform.rotation == Quaternion.Euler(0, 0, -90)) { return true; }
        elaspedtime += Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(new Vector3(0, 0, -90)), elaspedtime);
        return false;

    }
    private void moveToPlayer()
    {
        timeSinceLastMove += Time.deltaTime;

        float vertical = player.transform.position.y - gameObject.transform.position.y;
        if (vertical > 1)
        {
            isGrounded = false;
            isJumping = true;
            timeInJump = 0f;
        }
        if (isJumping)
        {
            timeInJump += Time.deltaTime;
            if (timeInJump <= jumpTime)
            {
                gameObject.transform.position += Vector3.up * 5 * Time.deltaTime;
            }
            if (timeInJump > jumpTime && isGrounded == false)
            {
                gameObject.transform.position -= Vector3.up * 5 * Time.deltaTime;
            }
        }

        if (movingTo.x == gameObject.transform.position.x || (movingFrom == Vector3.zero && movingTo == Vector3.zero) || timeSinceLastMove > 5)
        {
            timeSinceLastMove = 0;
            float horizontal = player.transform.position.x - gameObject.transform.position.x;

            if (horizontal == 0)
            {
                ani.SetBool("walking", false);
            }
            if (horizontal > 0)
            {
                spRdr.flipX = false;
                ani.SetBool("walking", true);
            }
            if (horizontal < 0)
            {
                spRdr.flipX = true;
                ani.SetBool("walking", true);

            }
            movingTo = player.transform.position;
            movingFrom = gameObject.transform.position;
            Debug.Log(movingFrom);
        }
        if (movingTo.x != gameObject.transform.position.x )
        {
            float f = Vector3.Lerp(movingFrom, movingTo, timeSinceLastMove/10).x;
            Vector3 newPos = new Vector3(f, gameObject.transform.position.y, 0);
            gameObject.transform.position = newPos;
        }
        
    }
}
