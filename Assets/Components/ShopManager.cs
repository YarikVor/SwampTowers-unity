using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; } = null;
    

    [Min(0)]
    public int money = 0;
    public MoneyVisibly moneyVisibly;
    
    private void Awake()
    {
        if ((Instance ??= this) != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        money = GetComponent<LevelHolder>().level.startMoney;
    }

    [NonSerialized]
    private ShopItem selectedShopItem = null;

    public ShopItem SelectedShopItem
    {
        get => selectedShopItem;
        set
        {
            selectedShopItem = value;
            OnSelectShopItem?.Invoke(value);
        }
    }

    public event Action<ShopItem> OnSelectShopItem; 

    public void AddMoney(int value)
    {
        money += value;
    }

    public void AddMoneyWithEffect(int value, Vector3 spawn)
    {
        AddMoney(value);

        var instantiate = Instantiate(moneyVisibly, spawn, Quaternion.identity);

        instantiate.Init(value);
    }
}
