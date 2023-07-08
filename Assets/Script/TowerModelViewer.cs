using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerModelViewer
{
    public readonly EnemyType enemyType;
    public readonly int damage;
    public readonly float radius;
    public readonly float cooldown;
    public readonly int countMonstersDamage;
    public readonly int sell;
    public readonly int update;
    public readonly Sprite icon;
    public readonly string description;
    public readonly string name;
    public readonly int currentLevel;

    public TowerModelViewer(
    	EnemyType enemyType, 
    	int damage, 
    	float radius, 
    	float cooldown, 
    	int countMonstersDamage, 
    	int sell, 
    	int update, 
    	Sprite icon, 
    	string description,
        string name,
        int currentLevel
    )
    {
        this.enemyType = enemyType;
        this.damage = damage;
        this.radius = radius;
        this.cooldown = cooldown;
        this.countMonstersDamage = countMonstersDamage;
        this.sell = sell;
        this.update = update;
        this.icon = icon;
        this.description = description;
        this.name = name;
        this.currentLevel = currentLevel;
    }

    public bool HasNextLevel => update != 0;
}