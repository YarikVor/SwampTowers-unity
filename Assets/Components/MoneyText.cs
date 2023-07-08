using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MoneyText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    private ShopManager _shopManager;

    private int _lastMoney;

    void Start()
    {
        _shopManager = ShopManager.Instance;
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    private void UpdateText()
    {
        _textMeshPro.text = _shopManager.money.ToString();
    }
    
    void Update()
    {
        if (_lastMoney != _shopManager.money)
        {
            UpdateText();
        }

        _lastMoney = _shopManager.money;
    }
}
