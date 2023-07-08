using System.Linq;
using UnityEngine;

public class ShopUIComponent : MonoBehaviour
{
    public static ShopUIComponent Instance { get; private set; }
    
    public TowerInfoCollection collection;
    private TowerInfoProvider manager;

    public GameObject Item;
    public ShopItem[] shopItemCollection;

    private void Awake()
    {
        if ((Instance ??= this) != this)
            Destroy(gameObject);
    }

    void Start()
    {
        manager = new TowerInfoProvider(collection);

        shopItemCollection = manager.ShopItems.ToArray();

        foreach (var item in shopItemCollection)
        {
            Instantiate(Item, transform)
                .GetComponent<ShopItemComponent>()
                .Init(item, this);
        }
    }

    public void InvertVisibly()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OnClickItem(ShopItem shopItem)
    {
        ShopManager.Instance.SelectedShopItem = shopItem;
    }
}
