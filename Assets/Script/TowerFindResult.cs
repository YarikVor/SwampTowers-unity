using JetBrains.Annotations;

public class TowerFindResult
{
    public TowerInfo source;

    public int indexOfItem;

    public TowerFindResult(TowerInfo source, int indexOfItem)
    {
        this.source = source;
        this.indexOfItem = indexOfItem;
    }

    public TowerUpgradeInfo CurrentItem => source.infoItems[indexOfItem];

    [CanBeNull]
    public TowerUpgradeInfo NextItem => source.infoItems.GetElementOrNullByIndex(indexOfItem + 1);
}