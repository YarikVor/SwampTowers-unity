using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[Serializable]
public class TowerUpgradeInfo
{
    public static readonly TowerUpgradeInfo Empty = new TowerUpgradeInfo()
    {
        buildTime = 0,
        salePrice = 0,
        tower = null,
        buyPrice = 0
    };

    [NotNull]
    public GameObject tower = null!;

    [Min(0)]
    public int buyPrice;

    [Min(0)]
    public int salePrice;

    [Min(float.Epsilon)]
    public float buildTime;
}