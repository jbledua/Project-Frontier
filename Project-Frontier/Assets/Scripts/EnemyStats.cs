using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class doesn't even need to be a monobehaviour!
//but it wouldn't work right if i tried to treat it as a normal c# class!
//I hate c#
public class EnemyStats : MonoBehaviour
{
    public int CurrentHP;
    public int MaxHP;
    public int Defense;
    public int OnImpactDMG = 1;
    public int getHp() { return CurrentHP; }
    public void setHp(int hp) { if (hp < MaxHP) { CurrentHP = hp; } }
    public void setMaxHP(int hp) { MaxHP = hp; }   
    public int getMaxHP (){ return MaxHP; }
    public void setDefense(int defense) { Defense = defense; }
    public void Hit(int damage)
    {
        if (damage - Defense <= 0) CurrentHP -= 1;
        else CurrentHP -= (damage - Defense);
    }

}
