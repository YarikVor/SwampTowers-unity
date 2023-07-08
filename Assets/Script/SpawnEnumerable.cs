using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnEnumerable : IEnumerable<GameObject>
{
    private readonly Level _level;
    public readonly int CountWaves;
    public readonly int CountMonsters;

    public int CurrentPositionWave { get; private set; }
    public Wave CurrentWave { get; private set; }
    public float SpawnTime { get; private set; }
    public int CountSpawnMonsters { get; private set; } = 0;

    public SpawnEnumerable(Level level)
    {
        _level = level;
        CurrentPositionWave = 0;
        CountWaves = _level.waves.Length;
        
        CountMonsters = _level.waves
            .Select(
                w => w.subWaves
                    .Select(s => s.count)
                    .DefaultIfEmpty()
                    .Sum()
            )
            .DefaultIfEmpty()
            .Sum();

    }


    public IEnumerator<GameObject> GetEnumerator()
    {
        for (var levelLength = _level.waves.Length; CurrentPositionWave < levelLength; CurrentPositionWave++)
        {
            CurrentWave = _level.waves[CurrentPositionWave];
            SpawnTime = CurrentWave.spawnTime;
            foreach (var subWave in CurrentWave.subWaves)
            {
                for (int i = 0; i < subWave.count; i++)
                {
                    yield return subWave.monster;
                    CountSpawnMonsters++;
                }
            }
        }

        CurrentPositionWave--;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public float MonsterFillPercentage => CountSpawnMonsters / (float)CountMonsters;
}