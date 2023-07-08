using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public Transform Bar;

    public int maxHealth = 1;

    public void Init(EnemyHealth enemyHealth)
    {
        this.enemyHealth = enemyHealth;
    }

    void Start()
    {
        maxHealth = enemyHealth.EnemyInfo.maxHealth;
        Debug.Log($"maxHealth:{maxHealth}");
    }

    public float PrecentHealth => enemyHealth.health / (float)maxHealth;

    void Update()
    {
        Debug.Log(PrecentHealth);
        var scale = Bar.localScale;
        scale.x = PrecentHealth;
        Bar.localScale = scale;
    }

}