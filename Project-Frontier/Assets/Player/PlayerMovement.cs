using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float checkRadius = 0.1f;


    public GameObject spawnPoint;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float moveInput;
    private bool isGrounded;
    private bool jumpRequest;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform firePoint_x;
    public float bulletSpeed = 10f;

    public float respawnDelay = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Move player to spawn point
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }

        OnPlayerKilled();
    }

    // New method for handling player death
    public void OnPlayerKilled()
    {

        StartCoroutine(RespawnAfterDelay(respawnDelay));
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        // Disable player control and visuals
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        // Call the OnSpawn method of the Spawner
        Spawner spawner = spawnPoint.GetComponent<Spawner>();
        if (spawner != null)
        {
            spawner.OnSpawn();
        }

        // Wait for the delay
        yield return new WaitForSeconds(delay);

        // Move player to the active spawn point
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
            
        }

        // Enable player control and visuals

        GetComponent<PlayerInput>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        

    }

    void Update()
    {
        // Check if the player is on the ground using the Ground Check object
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);


        // Update the animator parameters
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsGrounded", isGrounded);

        // Flip the sprite based on the movement direction
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        // Move the player left or right
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Execute jump if requested
        if (jumpRequest)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpRequest = false;
        }
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FireBullet();
        }
    }

        public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            jumpRequest = true;
        }
    }

    private void FireBullet()
    {
        GameObject bullet;
        Rigidbody2D bulletRb;
        if (spriteRenderer.flipX)
        {
            // Instantiate bullet
           bullet = Instantiate(bulletPrefab, firePoint_x.position, firePoint_x.rotation);
           bulletRb = bullet.GetComponent<Rigidbody2D>();
        }
        else
        {
            bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bulletRb = bullet.GetComponent<Rigidbody2D>();
        }
        

        // Set bullet velocity
        float direction = spriteRenderer.flipX ? -1 : 1;
        bulletRb.velocity = new Vector2(direction * bulletSpeed, 0);

        // Play firing animation
        animator.SetTrigger("Fire");

        // Destroy bullet after some time
        Destroy(bullet, 2f);
    }

    // New method for updating spawn point
    public void UpdateSpawnPoint(GameObject newSpawnPoint)
    {
        if (newSpawnPoint != null)
        {
            spawnPoint = newSpawnPoint;
        }
    }

    // New method for handling player interaction
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelExit[] levelExits = FindObjectsOfType<LevelExit>();
            foreach (LevelExit exit in levelExits)
            {
                exit.Interact();
            }
        }
    }
}
