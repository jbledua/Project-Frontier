using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.collider.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.OnPlayerKilled();
            }
        }
    }
}
