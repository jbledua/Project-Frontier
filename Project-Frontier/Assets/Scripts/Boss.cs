using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//This bundle of joy is the class that controls boss behavior
//Really should be split into like 3 classes
//Would have been alot easier if i had just done that in the first place but alas we are here
//Honestly the stupidity in this class is so bad i recommened not looking at it.
public class Boss : MonoBehaviour
{
    
    public enum states { Passive, Active, Attacking, Passive_1, Passive_2};
    public states state;
    public enum attacks { Beam, Spray};
    public attacks attackType = attacks.Beam;
    private int attackPhase = 0;
    private int attackWay = 3;
    private int numberOfAttacks = 5;
    public GameObject[] attackProjectiles;

    private List<GameObject> minions = new List<GameObject>();
    public GameObject minion;
    public GameObject shield;
    private List<GameObject> ActiveProjectiles = new List<GameObject>();

    GameObject player;
    EnemyStats stats;
    Animator ani;

    private float stateDuration = 1.0f;
    public float timeUntilStateChange = 0.0f;
    private bool hasAttacked = false;

    private static int Iframes = 90;
    private int UpdatesSinceDmg = Iframes;
    private float UpdatesSinceAttack = 10f;

    public float elaspedtime = 0.0f;
    public bool isMoving = false;
    private Vector3 initialPos = Vector3.zero;
    private Quaternion initialRot = Quaternion.identity;
    
    //I wasn't gonna do some cool math trick to get the cardinals!
    public Vector2[] thisISdumb =
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
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        stats = gameObject.GetComponent<EnemyStats>();
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
                attackWay = Random.Range(0, 3);
                if (MoveToCenter() && !isMoving)
                {
                    if (attackWay == 0)
                    {
                        for (int i = 0; i < thisISdumb.Length; i++)
                        {
                            thisISdumb[i] = rotate(thisISdumb[i], 0.12f);
                        }
                        for (int j = 0; j < 8; j++)
                        {
                            Debug.Log("Shooting");
                            GameObject bullet = Instantiate(attackProjectiles[0], this.transform.position + (Vector3)(thisISdumb[j] * 2f), Quaternion.Euler(0, 0, 45 * j));
                            bullet.GetComponent<Bullet>().isPlayers = false;
                            bullet.transform.localScale = Vector3.one * 5f;
                            bullet.GetComponent<Rigidbody2D>().velocity = thisISdumb[j] * 50;
                            ActiveProjectiles.Add(bullet);
                        }
                        numberOfAttacks--;
                        if (numberOfAttacks <= 0) { hasAttacked = true; attackType = (attacks)Random.Range(0, 2); numberOfAttacks = Random.Range(0, 3); attackWay = 3; }
                    }
                    if(attackWay == 1)
                    {
                        Vector2[] thisIS =
                        {
                        Vector2.up,
                        Vector2.right,
                        Vector2.down,
                        Vector2.left

                            };
                        for (int j = 0; j < 4; j++)
                        {
                            Debug.Log("Shooting");
                            GameObject bullet = Instantiate(attackProjectiles[0], this.transform.position + (Vector3)(thisIS[j] * 2f), Quaternion.Euler(0, 0, 0));
                            bullet.GetComponent<Bullet>().isPlayers = false;
                            bullet.transform.localScale = new Vector3(1,4,1) * 5f;
                            bullet.GetComponent<Rigidbody2D>().velocity = thisIS[j] * 50;
                            ActiveProjectiles.Add(bullet);
                        }
                        hasAttacked = true; attackType = (attacks)Random.Range(0, 2); numberOfAttacks = Random.Range(0, 3); attackWay = 3;
                    }
                    if (attackWay == 2)
                    {
                        Vector2[] thisIS =
                         {
                        Vector2.up,
                        Vector2.right,
                        Vector2.down,
                        Vector2.left

                            };
                        for (int j = 0; j < 4; j++)
                        {
                            Debug.Log("Shooting");
                            GameObject bullet = Instantiate(attackProjectiles[0], this.transform.position + (Vector3)(thisIS[j] * 2f), Quaternion.Euler(0, 0, 0));
                            bullet.GetComponent<Bullet>().isPlayers = false;
                            bullet.transform.localScale = new Vector3(4, 1, 1) * 5f;
                            bullet.GetComponent<Rigidbody2D>().velocity = thisIS[j] * 50;
                            ActiveProjectiles.Add(bullet);
                        }
                        hasAttacked = true; attackType = (attacks)Random.Range(0, 2); numberOfAttacks = Random.Range(0, 3); attackWay = 3;
                    }

                }
            }
            else if (attackType.Equals(attacks.Beam))
            {
                if (attackPhase == 0)
                {
                    int i = Random.Range(1, 3);
                    Debug.Log("Attack Phase: " + i);
                    if (attackWay == 3)
                    {
                        if (player.transform.position.x > 0) { attackWay = 1; }
                        else { attackWay = 0; }
                    }
                    if (attackWay == 0)
                    {
                        if (MoveStartLeft()) { attackPhase = i; }
                    }
                    if (attackWay == 1)
                    {
                        
                        if (MoveStartRight()) { attackPhase = i; }
                    }
                }
                if (attackPhase == 1)
                {
                    MoveToCenter();
                    attackPhase = 3;
                    if (isMoving)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            Debug.Log("Shooting");
                            GameObject bullet = Instantiate(attackProjectiles[0], this.transform.position + (Vector3.down * (j * 0.25f)), Quaternion.Euler(0, 0, 0));
                            bullet.GetComponent<Bullet>().isPlayers = false;
                            bullet.transform.localScale = Vector3.one * 2f;
                            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * 50;
                            ActiveProjectiles.Add(bullet);
                        }
                    }
                    else
                    {
                        hasAttacked = true;
                        attackPhase = 0;
                        attackWay = 3;
                        
                        attackType = attacks.Spray;
                    }
                }
                if(attackPhase == 2)
                {
                    if(attackWay == 0) { MoveStartRight(); }
                    if(attackWay == 1) { MoveStartLeft(); }

                    attackPhase = 4;
                    if (isMoving)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            Debug.Log("Shooting");
                            GameObject bullet = Instantiate(attackProjectiles[0], this.transform.position + (Vector3.down * (j * 0.25f)), Quaternion.Euler(0, 0, 0));
                            bullet.GetComponent<Bullet>().isPlayers = false;
                            bullet.transform.localScale = Vector3.one * 2f;
                            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * 50;
                            ActiveProjectiles.Add(bullet);
                        }
                    }
                    else
                    {
                        hasAttacked = true;
                        attackPhase = 0;
                        attackWay = 3;
                        
                        attackType = attacks.Spray;
                    }
                }
                if (attackPhase == 3)
                {
                    attackPhase = 1;
                }
                if (attackPhase == 4)
                {
                    attackPhase = 2; //oh what i would give for a goto!
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
        if (state == states.Passive_2)
        {
            Debug.Log(minions.Count);
            for(int i = 0; i < minions.Count; i++)
            {
                if (minions[i] == null) { minions.Remove(minions[i]); }
            }
            if(minions.Count <= 0)
            {
                ani.SetInteger("phase", 1);
                ani.SetTrigger("anger");
                state = states.Attacking;
                shield.SetActive(false);
            }

         }
         if(state == states.Passive_1)
        {
            if (MoveToCenterTop()) {
                
                shield.SetActive(true);
                while(minions.Count < 5)
                {
                    minions.Add(Instantiate(minion, new Vector3(Random.Range(-16, 16), -7, 0), Quaternion.identity));
                }
                state = states.Passive_2;
            }
        }
        if (stats.getHp() <= 0 && Quaternion.identity == initialRot)
        {
            initialRot = gameObject.transform.rotation;
            deathAni();
            ani.SetTrigger("death");
            ani.SetInteger("phase", 2);
            return;
        }
        if (initialRot != Quaternion.identity)
        {
            if (deathAni()) { Destroy(gameObject); }

        }

    }
    private void FixedUpdate()
    {
        if (UpdatesSinceDmg <= Iframes) UpdatesSinceDmg++;
        if (ani.GetInteger("phase") == 1 && !shield.active) { UpdatesSinceAttack += Time.deltaTime; }
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
    private bool deathAni()
    {
        if (gameObject.transform.rotation == Quaternion.Euler(0, 0, 120)) { return true; }
        elaspedtime += Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(new Vector3(0, 0, 120)), elaspedtime/8);
        return false;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.CompareTag("Bullet") && collision.gameObject.GetComponent<Bullet>().isPlayers == true && UpdatesSinceDmg > Iframes)
            {
                UpdatesSinceDmg = 0;
                ani.SetTrigger("Damage");
                stats.Hit(collision.gameObject.GetComponent<Bullet>().dmg);
                if(state== states.Passive) { state = states.Active; timeUntilStateChange = stateDuration; }
                if(stats.getHp() < (stats.getMaxHP()/2) && UpdatesSinceAttack >= 10f) {
                state = states.Passive_1;
                UpdatesSinceAttack = 0;
                }
            }
    
    }
    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }


}
