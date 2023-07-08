using UnityEngine;

[CreateAssetMenu(fileName = "TowerLevel", menuName = "Tower Defense/Tower Level")]
public class TowerInfoCollection : ScriptableObject
{
    public TowerInfo[] towerLevelInfos;

    [ContextMenu("Calculation Sell Values")]
    public void CalcSellForTowers()
    {
        foreach (var towerLevelInfo in towerLevelInfos)
        {
            var infoItems = towerLevelInfo.infoItems;
            float saleValue = 0;
            for (
                int index = 0, itemLength = infoItems.Length;
                index < itemLength;
                index++
            )
            {
                var item = infoItems[index];
                saleValue += item.buyPrice;
                item.salePrice = (int)(saleValue * MoneySellKoef(index + 1));
            }
        }
    }

    private static float MoneySellKoef(int level)
    {
        return level switch
        {
            <= 1 => 0.7f,
            _ => 1 - 0.04f * (level + 1)
        };
    }
}