using System;
using UnityEngine;

[Serializable]
public class TowerСharacter 
{
    public EnemyType target;
    [Min(0)]
    public int damage;
    [Min(0.0001f)]
    public float cooldown;
    [Min(0)]
    public int countMonstersDamage = 1;
}