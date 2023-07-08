using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Tower Defense/Level", order = 3)]
public class Level : ScriptableObject
{
    [SerializeField] public Wave[] waves;

    [Min(1)] public int startMoney;
    [Min(1)] public int life;

    public SpawnEnumerable GetSpawnEnumerable()
    {
        return new SpawnEnumerable(this);
    }


}

