using System;
using UnityEngine;

[Serializable]
public class ShopItem
{
    public Sprite icon;
    public GameObject tower;
    [Min(1)] 
    public EnemyType targetType;
    public int price;
    public string name;
    [TextArea]
    public string description;
    public float buildTime;
}