using System;
using UnityEngine;

[Serializable]
public class TowerShopItem
{
    public Sprite icon;
    public EnemyType targetType;
    public string name;
    [TextArea]
    public string description;
}