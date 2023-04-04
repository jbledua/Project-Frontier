using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int CurrentHP;
    public int MaxHP;
    public int Defense;

    public int getHp() { return CurrentHP; }
    public void setHp(int hp) { if (hp < MaxHP) { CurrentHP = hp; } }
    public void setMaxHP(int hp) { MaxHP = hp; }   
    public int getMaxHP (){ return MaxHP; }
    public void setDefense(int defense) { Defense = defense; }
    public void Hit(int damage)
    {
        CurrentHP -= (damage - Defense);
    }


}
