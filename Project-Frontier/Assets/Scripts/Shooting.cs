using System.Collections;
using System.Linq;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//this was my solution to dealing with rotation and physics! IT's DUMB af
//but it works! also why 6 cubes follow the player sprite (they are barrel locations!) never finished this class!
//All i needed was something that could shoot.
public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    public Rigidbody2D rb;
    public SpriteRenderer spRdr;
    public Animator ani;

    public GameObject left;
    public GameObject top_left;
    public GameObject bottom_left;

    public GameObject right;
    public GameObject top_right;
    public GameObject bottom_right;

    public enum Direction { High, Low, Forward};
    public Direction state = Direction.Forward;
    private List<GameObject> ActiveBullets = new List<GameObject>();

    void Start()
    {
        spRdr = GetComponent<SpriteRenderer>();
        rb= GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0) && ActiveBullets.Count < 100)
        {
            GameObject i;
            if (spRdr.flipX)
            {
                i = Instantiate(bullet, left.transform.position, left.transform.rotation);
                i.GetComponent<Bullet>().isPlayers = true;
                i.GetComponent<Bullet>().dmg = 5;
                i.transform.localScale= Vector3.one;
                Rigidbody2D i_b = i.GetComponent<Rigidbody2D>();
                i_b.velocity = Vector3.left * 50;
                
            }
            else
            {
                i = Instantiate(bullet, right.transform.position, right.transform.rotation);
                i.GetComponent<Bullet>().isPlayers = true;
                i.GetComponent<Bullet>().dmg = 5;
                i.transform.localScale = Vector3.one;
                Rigidbody2D i_b = i.GetComponent<Rigidbody2D>();
                i_b.velocity = Vector3.right * 50;
            }
            ani.SetTrigger("shootforward");
            ActiveBullets.Add(i);  
        }
    }
}
