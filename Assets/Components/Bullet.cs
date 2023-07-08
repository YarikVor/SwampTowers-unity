using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    public Vector2 Velocity;
    public EnemyType TargetType;
    public float speed;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.TransformVector(Velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHeart = other.GetComponent<EnemyHealth>();

        if (!enemyHeart)
            return;
        
        if (!enemyHeart.EnemyInfo.enemyType.IsAttack(TargetType))
            return;
        
        
    }
}
