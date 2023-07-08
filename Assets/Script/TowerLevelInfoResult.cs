using UnityEngine;

public class TowerLevelInfoResult
{
    public readonly int sellPrice;
    public readonly int updatePrice;
    public readonly GameObject nextLevelTower;
    public readonly int currentLevel;
    public readonly float buildTime;

    public TowerLevelInfoResult(
        int sellPrice,
        int updatePrice,
        GameObject nextLevelTower,
        int currentLevel,
        float buildTime
    )
    {
        this.sellPrice = sellPrice;
        this.updatePrice = updatePrice;
        this.nextLevelTower = nextLevelTower;
        this.currentLevel = currentLevel;
        this.buildTime = buildTime;
    }

    public bool HasNextLevel => nextLevelTower;
}