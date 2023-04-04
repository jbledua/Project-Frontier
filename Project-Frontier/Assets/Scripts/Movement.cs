using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

//Not adding anything else to this class, Feel free to replace everything here!
//I have hated dealing with this janky physics system and Transform.translate since unity 3.0!
public class Movement : MonoBehaviour
{
    Animator ani;
    Rigidbody2D rb;
    SpriteRenderer ren2d;

    public float jp_height = 1.0f;

    private float jumpTime = 1.0f;
    private float timeInJump = 0.0f;
    private bool isJumping = false;
    private bool isFalling = false;
    public bool isGrounded = false;

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
        if (Input.GetKey(KeyCode.W) && isJumping == false)
        {
            isGrounded = false;
            isJumping= true;
            timeInJump = 0f;
        }
        if(isJumping)
        {
            timeInJump += Time.deltaTime;
            if(timeInJump <= jumpTime && !isFalling)
            {
                rb.transform.position += Vector3.up * 5 * Time.deltaTime;
            }
            if(timeInJump > jumpTime && isGrounded == false) {
                isFalling = true;
            }
            if(timeInJump > jumpTime && isGrounded == true) { isJumping = false; }
        }
        if (isFalling && !isGrounded)
        {
            rb.transform.position -= Vector3.up * 5 * Time.deltaTime;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);   
        if (collision.gameObject.tag == "Ground") { isGrounded = true; isFalling = false; }
        if (collision.gameObject.tag == "Platform") { isGrounded = true; isFalling = false; }
    }

}
