using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemComponent : MonoBehaviour
{
    public ShopUIComponent shopUIComponent;
    private ShopItem _shopItem;

    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI descriptionText;

    public Image enemyIcon;
    public Image bossIcon;
    public Image groundIcon;
    public Image flyIcon;

    public void Init(ShopItem shopItem, ShopUIComponent shopUIComponent)
    {
        icon.sprite = shopItem.icon;
        nameText.text = shopItem.name;
        priceText.text = shopItem.price.ToString();
        descriptionText.text = shopItem.description;

        _shopItem = shopItem;

        this.shopUIComponent = shopUIComponent;

        shopItem.targetType.SetIcons(enemyIcon, bossIcon, groundIcon, flyIcon);
    }

    public void OnClick()
    {
        shopUIComponent.OnClickItem(_shopItem);
    }
}