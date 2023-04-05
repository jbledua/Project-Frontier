using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    GameObject HealthBar;
    EnemyStats stats;

    float initialScale;
    void Start()
    {
        stats = gameObject.GetComponent<EnemyStats>();
        foreach(Transform t in gameObject.transform)
        {
            if (t.CompareTag("HealthBar"))
            {
                HealthBar = t.gameObject;
                initialScale = HealthBar.transform.localScale.x;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(stats.getHp() <= 0)
        {
            HealthBar.SetActive(false);
        }
        HealthBar.transform.localScale = new Vector2(Normalize(stats.getMaxHP(),stats.getHp()), HealthBar.transform.localScale.y);
    }
    float Normalize(float maxHp, float curHp)
    {
        return (curHp / maxHp) * initialScale;
    }
}
