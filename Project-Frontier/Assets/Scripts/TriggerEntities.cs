using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEntities : MonoBehaviour
{
    GameObject parent;
    List<GameObject> enemies = new List<GameObject>();
    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        foreach(Transform t in parent.transform)
        {
            if (t.CompareTag("Enemy")) { enemies.Add(t.gameObject); }
            t.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach(GameObject g in enemies)
            {
                g.SetActive(true);
            }
        }
        
    }
}
