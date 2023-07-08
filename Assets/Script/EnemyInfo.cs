
//Монстр: Швидкість, Життя, Максимальне життя, Атака, Монети, Тип 

using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EnemyInfo
{
    [Min(0)]
    public float speed = 1;
    
    [Min(0)]
    public int health;

    [Min(1)]
    public int maxHealth;
    
    [Min(0)]
    public int damage = 1;
    
    [Min(0)]
    public int money = 1;
    
    public EnemyType enemyType;
    
    public float RelativeHealth => health / (float)maxHealth;
}
