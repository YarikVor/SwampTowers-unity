using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyVisibly : MonoBehaviour
{
    public void Init(int value)
    {
        GetComponentInChildren<TextMeshPro>().text = $"+{value}";
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
