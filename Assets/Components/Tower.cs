using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Tower : MonoBehaviour
{
    public TowerСharacter info;

    public GameObject prefab = null;

    public float Radius
    {
        get => transform.localScale.x;
        set => transform.localScale = new Vector3(value, value / 2, 1);
    }
    
    [SerializeField]
    private float delay = 1;

    [Min(0)]
    public int countAvaibleDamage = 0;

    void Update()
    {
        if (!IsFullDamage)
            countAvaibleDamage = 0;

        if (delay <= 0)
        {
            countAvaibleDamage = info.countMonstersDamage;
            delay += info.cooldown;
        } 
        else if (!IsDamage)
        {
            delay -= Time.deltaTime;
        }
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!IsDamage)
            return;
        
        EnemyHealth enemyHeart = other.GetComponent<EnemyHealth>();

        if (!other)
            return;

        if (!enemyHeart.EnemyInfo.enemyType.IsAttack(info.target))
            return;

        enemyHeart.TakeHearts(info.damage);
        countAvaibleDamage--;
    }

    public bool IsDamage => countAvaibleDamage != 0;
    public bool IsFullDamage => countAvaibleDamage == info.countMonstersDamage;
}
