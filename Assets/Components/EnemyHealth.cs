using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(EnemyInfoHolder))]
[AddComponentMenu("Tower Defense/Enemy/Enemy Health")]
public class EnemyHealth : MonoBehaviour
{
    public EnemyInfoHolder EnemyInfoHolder;

    public EnemyInfo EnemyInfo => EnemyInfoHolder.EnemyInfo;
    
    public int health;

    public Transform spriteTransform;
    

    void Start()
    {
        EnemyInfoHolder = GetComponent<EnemyInfoHolder>();
        
        spriteTransform = GetComponentsInChildren<Transform>()[1];
        Debug.Assert(spriteTransform != transform);

        UpdateHealth();
    }

    private void UpdateHealth()
    {
        health = EnemyInfo.health;
    }

    public void TakeHearts(int damage)
    {
        health -= damage;
        if (health <= 0)
            OnDead();

        spriteTransform.localScale = HealthToVectorScale(health / (float)EnemyInfo.maxHealth);
    }

    public static float HealthToScale(float input)
    {
        return input switch
        {
            < 0.25f => 0.5f,
            < 0.5f => 0.65f,
            < 0.75f => 0.8f,
            < 0.95f => 0.95f,
            _ => 1f
        };
    }

    public static Vector3 HealthToVectorScale(float input)
    {
        var scale = HealthToScale(input);

        return new Vector3(scale, scale);
    }

    private void OnDead()
    {
        ShopManager.Instance.AddMoneyWithEffect(EnemyInfo.money, transform.position);
        EnemyInfoHolder.spawner.AddKill();
        Destroy(gameObject);
    }
}