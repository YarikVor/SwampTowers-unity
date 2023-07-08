using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SubWave
{
    [Min(1)]
    public int count = 7;

    public GameObject monster;
    
    public EnemyInfo enemyInfo;
}
