using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    [SerializeField]
    public SubWave[] subWaves;

    [Min(0)]
    public float spawnTime;

    [Min(0)]
    public float pause;
}