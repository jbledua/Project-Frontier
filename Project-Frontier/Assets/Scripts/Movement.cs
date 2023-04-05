using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not adding anything else to this class, Feel free to replace everything here!
//I have hated dealing with this janky physics system and Transform.translate since unity 3.0!
public class Movement : MonoBehaviour
{
    Animator ani;
    Rigidbody2D rb;
    SpriteRenderer ren2d;

    public float jp_height = 1.0f;
    private int curFrames = 0;
    public int jumpFrames = 10;
    public bool isGrounded = false;
    public bool isJumping = false;
    public float elaspedTime = 0f;
    private Vector2 initialPos = Vector2.zero;
    private Vector2 finalPos = Vector2.zero;
    public float speed = 1.0f;
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ren2d = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if(horizontal == 0)
        {
            ani.SetTrigger("idle");
        }
        if(horizontal > 0)
        {
            
            ren2d.flipX = false;
            ani.SetTrigger("walk");
            rb.transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (horizontal < 0)
        {
           
            ren2d.flipX = true;
            ani.SetTrigger("walk");
            rb.transform.position += Vector3.left * speed * Time.deltaTime;
        }
        
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            isJumping = true;
        }
        if(isJumping) {
            Debug.Log("jump");
            jump(); 
            curFrames++; 
            if(curFrames >= jumpFrames) { isJumping = false; curFrames = 0; }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);   
        if (collision.gameObject.tag == "Ground") { isGrounded = true;}
        if (collision.gameObject.tag == "Platform") { isGrounded = true;}
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") { isGrounded = false;}
        if (collision.gameObject.tag == "Platform") { isGrounded = false;}
    }
    private void jump()
    {
        rb.AddForce(Vector3.up * jp_height * Time.deltaTime, ForceMode2D.Impulse);
    }
}
