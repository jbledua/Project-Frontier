using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawerTrigger : MonoBehaviour
{
    public GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.UpdateSpawnPoint(spawner);
            }
        }
    }
}
