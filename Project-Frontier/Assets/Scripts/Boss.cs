using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Boss : MonoBehaviour
{
    
    public enum states { Passive, Active, Attacking};
    public states state;
    public enum attacks { Beam, Spray};
    public attacks attackType = attacks.Beam;
    private int attackPhase = 0;
    public GameObject[] attackProjectiles;

    private List<GameObject> ActiveProjectiles = new List<GameObject>();

    public GameObject player;

    private float stateDuration = 1.0f;
    public float timeUntilStateChange = 0.0f;
    private bool hasAttacked = false;


    public float elaspedtime = 0.0f;
    public bool isMoving = false;
    private Vector3 initialPos = Vector3.zero;
    Animator ani;

    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        state = states.Passive;
        gameObject.transform.position = initialPos;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(state);
        
        if (state.Equals(states.Attacking) && !hasAttacked)
        {
            Debug.Log("Attacking");
            if (attackType.Equals(attacks.Spray))
            {
                if (MoveToCenter() && !isMoving)
                {
                    Vector2[] thisISdumb =
                    {
                    Vector2.up,
                    new Vector2(0.5f,0.5f),
                    Vector2.right,
                    new Vector2(0.5f,-0.5f),
                    Vector2.down,
                    new Vector2(-0.5f, -0.5f),
                    Vector2.left,
                    new Vector2(-0.5f, 0.5f)

                };
                    for (int j = 0; j < 8; j++)
                    {
                        Debug.Log("Shooting");
                        GameObject bullet = Instantiate(attackProjectiles[0], this.transform.position + (Vector3)(thisISdumb[j] * 2f), Quaternion.Euler(0, 0, 45 * j));
                        bullet.transform.localScale = Vector3.one * 2f;
                        bullet.GetComponent<Rigidbody2D>().velocity = thisISdumb[j] * 50;
                        ActiveProjectiles.Add(bullet);
                    }
                    attackType = attacks.Beam;
                    hasAttacked = true;
                }
            }
            else if (attackType.Equals(attacks.Beam))
            {
                if (attackPhase == 0)
                {
                    if (MoveStartLeft()) { attackPhase = 1; }
                }
                if (attackPhase == 1)
                {
                    MoveToCenter();
                    attackPhase = 2;
                    if (isMoving)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            Debug.Log("Shooting");
                            GameObject bullet = Instantiate(attackProjectiles[0], this.transform.position + (Vector3.down * (j * 0.25f)), Quaternion.Euler(0, 0, 0));
                            bullet.transform.localScale = Vector3.one * 2f;
                            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * 50;
                            ActiveProjectiles.Add(bullet);
                        }
                    }
                    else
                    {
                        hasAttacked = true;
                        attackPhase = 0;
                        attackType = attacks.Spray;
                    }
                }
                if (attackPhase == 2)
                {
                    attackPhase = 1;
                }
            }
        }

         if (timeUntilStateChange > 0.0f)
            {
                timeUntilStateChange -= Time.deltaTime;
            }
         if (state == states.Active && timeUntilStateChange <= 0.0f && !isMoving)
            {
                hasAttacked = false;
                state = states.Attacking;
                timeUntilStateChange = stateDuration;
            }
         if (state == states.Attacking && timeUntilStateChange <= 0.0f && !isMoving)
            {
                state = states.Active;
                timeUntilStateChange = stateDuration;
            }


        
    }
    bool MoveStartLeft()
    {
        Debug.Log("Moving To Left");
        if (gameObject.transform.position == new Vector3(-18, 4.5f, 0)) { isMoving = false; return true; }
        if (!isMoving) { initialPos = gameObject.transform.position; isMoving = true; elaspedtime = 0.0f; }
        //Debug.Log(Vector3.Lerp(initialPos, new Vector3(-18, 4.5f, 0), elaspedtime));
        elaspedtime += Time.deltaTime;
        this.gameObject.transform.position = Vector3.Lerp(initialPos, new Vector3(-18, 4.5f, 0), elaspedtime);
        return false;
    }
    bool MoveStartRight()
    {
        if (gameObject.transform.position == new Vector3(18, 4.5f, 0)) { isMoving = false; return true; }
        if (!isMoving) { initialPos = gameObject.transform.position; isMoving = true; elaspedtime = 0.0f; }
        //Debug.Log(Vector3.Lerp(initialPos, new Vector3(18, 4.5f, 0), elaspedtime));
        elaspedtime += Time.deltaTime;
        this.gameObject.transform.position = Vector3.Lerp(initialPos, new Vector3(18, 4.5f, 0), elaspedtime);
        return false;
    }
    bool MoveToCenterTop()
    {
        if (gameObject.transform.position == new Vector3(0, 4.5f, 0)) { isMoving = false; return true; }
        if (!isMoving) { initialPos = gameObject.transform.position; isMoving = true; elaspedtime = 0.0f; }
        //Debug.Log(Vector3.Lerp(initialPos, new Vector3(0, 4.5f, 0), elaspedtime));
        elaspedtime += Time.deltaTime;
        this.gameObject.transform.position = Vector3.Lerp(initialPos, new Vector3(0, 4.5f, 0), elaspedtime);
        return false;
    }
    bool MoveToCenter()
    {
        Debug.Log("Moving to center");
        if(gameObject.transform.position == Vector3.zero) { isMoving = false; return true; }
        if (!isMoving) { initialPos = gameObject.transform.position; isMoving = true; elaspedtime = 0.0f; }
        //Debug.Log(Vector3.Lerp(initialPos, new Vector3(0, 0, 0), elaspedtime));
        elaspedtime += Time.deltaTime;
        this.gameObject.transform.position = Vector3.Lerp(initialPos, new Vector3(0, 0, 0), elaspedtime);

        return false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.CompareTag("Bullet"))
            {
                ani.SetTrigger("Damage");
                if(state== states.Passive) { state = states.Active; timeUntilStateChange = stateDuration; }
            }
    
    }
}
