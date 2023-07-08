using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoUI : MonoBehaviour
{
    public static TowerInfoUI Instance { get; private set; }

    public const string SellFormat = "Sell\n{0}";
    public const string UpdateFormat = "Update\n{0}";

    public Image icon;
    public TextMeshProUGUI title;

    public Image entity;
    public Image boss;
    public Image fly;
    public Image ground;

    public TextMeshProUGUI description;

    public TextMeshProUGUI btnSellText;
    public TextMeshProUGUI btnUpdateText;

    public GameObject btnUpdate;
    
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI countDamageText;

    public GameObject tower;

    private void Awake()
    {
        if ((Instance ??= this) != this)
        {
            Destroy(gameObject);
        }

        gameObject.SetActive(false);
    }

    public void SetModel(TowerModelViewer model)
    {
        model.enemyType.SetIcons(entity, boss, ground, fly);
        damageText.text = model.damage.ToString();
        rangeText.text = model.radius.ToString();
        cooldownText.text = model.cooldown.ToString();
        countDamageText.text = model.countMonstersDamage.ToString();

        btnSellText.text = string.Format(SellFormat, model.sell);

        Debug.Log(model.update);

        if (model.HasNextLevel)
        {
            btnUpdateText.text = string.Format(UpdateFormat, model.update);
            btnUpdate.SetActive(true);
        }
        else
        {
            btnUpdate.SetActive(false);
        }

        icon.sprite = model.icon;

        title.text = $"{model.name} ({model.currentLevel})";
        description.text = model.description;
    }
}
