using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class TowerInfoProvider
{
    private readonly TowerInfoCollection _towerLevelInfoCollection;

    private TowerInfo[] TowerLevelInfos => _towerLevelInfoCollection.towerLevelInfos;

    public TowerInfoProvider(TowerInfoCollection collection)
    {
        Debug.Assert(collection != null);
        _towerLevelInfoCollection = collection;
    }

    [CanBeNull]
    public TowerLevelInfoResult GetTowerLevelInfoResult(GameObject gameObject)
    {
        var tower = GetTower(gameObject);
        return GetTowerLevelInfoResult(tower);
    }

    [CanBeNull]
    public TowerLevelInfoResult GetTowerLevelInfoResult(Tower tower)
    {
        var result = GetTowerFindResultByTower(tower);

        if (result == null)
            return null;

        return ToTowerLevelInfoResult(result);
    }



    private static TowerLevelInfoResult ToTowerLevelInfoResult(TowerFindResult result)
    {
        var towerLevel = result.CurrentItem;
        var nextTowerLevel = result.NextItem ?? TowerUpgradeInfo.Empty;
        var levelValue = result.indexOfItem + 1;

        return ToTowerLevelInfoResult(towerLevel, nextTowerLevel, levelValue);
    }

    private static TowerLevelInfoResult ToTowerLevelInfoResult(
        TowerUpgradeInfo towerLevel,
        TowerUpgradeInfo nextTowerLevel,
        int levelValue
    )
    {
        return new TowerLevelInfoResult(
            towerLevel.salePrice,
            nextTowerLevel.buyPrice,
            nextTowerLevel.tower,
            levelValue,
            nextTowerLevel.buildTime
        );
    }

    public GameObject GetFirstLevelTower(GameObject gameObject)
    {
        Tower tower = GetTower(gameObject);
        return GetFirstLevelTower(tower);
    }

    public GameObject GetFirstLevelTower(Tower tower)
    {
        var result = GetTowerFindResultByTower(tower);

        return result?.source.infoItems[0].tower;
    }


    private static Tower GetTower(GameObject gameObject)
    {
        var tower = gameObject.GetComponentInChildren<Tower>();

        if(tower == null)
            throw new Exception($"'{gameObject.name} isn't Tower'");

        return tower;
    }


    public ShopItem GetShopItemForTower(GameObject gameObject)
    {
        var tower = GetTower(gameObject);
        return GetShopItemForTower(tower);
    }

    public ShopItem GetShopItemForTower(Tower tower)
    {
        return ToShopItem(GetTowerFindResultByTower(tower).source);
    }

    public TowerFindResult GetTowerFindResultByTower(Tower tower)
    {
        foreach (var towerLevelInfo in TowerLevelInfos)
        {
            var towerLevels = towerLevelInfo.infoItems;

            for (int j = 0, length = towerLevels.Length; j < length; j++)
            {
                var towerLevel = towerLevels[j];
                var prefab = towerLevel.tower;

                if (tower.prefab == prefab)
                    return new TowerFindResult(towerLevelInfo, j);
            }
        }

        return null;
    }

    public TowerModelViewer GetTowerModelViewer(GameObject gameObject)
    {
        var tower = GetTower(gameObject);

        return GetTowerModelViewer(tower);
    }

    public TowerModelViewer GetTowerModelViewer(Tower tower)
    {
        var result = GetTowerFindResultByTower(tower);

        return ToTowerModelViewer(tower, result);
    }

    public static TowerModelViewer ToTowerModelViewer(Tower tower, TowerFindResult result)
    {
        var towerLevelInfoItem = result.CurrentItem;
        var shopItem = result.source.shopItem;
        var level = result.indexOfItem + 1;
        var nextTowerLevelInfoItem = result.NextItem 
            ?? TowerUpgradeInfo.Empty;

        return ToTowerModelViewer(tower, towerLevelInfoItem, shopItem, level, nextTowerLevelInfoItem);
    }

    public static TowerModelViewer ToTowerModelViewer(
        Tower tower, 
        TowerUpgradeInfo towerLevelInfoItem, 
        TowerShopItem shopItem,
        int level,
        TowerUpgradeInfo nextTowerLevelInfoItem
        )
    {
        var model = new TowerModelViewer(
            tower.info.target,
            tower.info.damage,
            tower.Radius,
            tower.info.cooldown,
            tower.info.countMonstersDamage,
            towerLevelInfoItem.salePrice,
            nextTowerLevelInfoItem.buyPrice,
            shopItem.icon,
            shopItem.description,
            shopItem.name,
            level
        );

        return model;
    }

    public IEnumerable<ShopItem> ShopItems =>
        _towerLevelInfoCollection
            .towerLevelInfos
            .Select(ToShopItem);

    public static ShopItem ToShopItem(TowerInfo info)
    {
        return ToShopItem(info.shopItem, info.infoItems[0]);
    }

    public static ShopItem ToShopItem(TowerShopItem shopItem, TowerUpgradeInfo updateInfo)
    {
        return new ShopItem()
        {
            icon = shopItem.icon,
            name = shopItem.name,
            targetType = shopItem.targetType,
            price = updateInfo.buyPrice,
            description = shopItem.description,
            tower = updateInfo.tower,
            buildTime = updateInfo.buildTime
        };
    }


}