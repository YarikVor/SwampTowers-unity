using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Shop", menuName = "Tower Defense/Shop", order = 0)]
public class Shop : ScriptableObject
{
    public ShopItem[] items;
}