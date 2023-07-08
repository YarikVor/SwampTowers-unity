using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    private Level _level;

    public Path path;

    public SpawnEnumerable spawnEnumerable;

    [CanBeNull]
    public Transform spawnEnemy;
    public GameObject HealthBarPrefab;

    public int CountWaves { get; private set; }
    public int CountMonsters { get; private set; }
    public int CurrentPositionWave { get; private set; }
    public Wave CurrentWave { get; private set; }
    public float SpawnTime { get; private set; }
    public int CountSpawnMonsters { get; private set; } = 0;

    public int CountKilledMonsters { get; private set; } = 0;

    public float MonsterFillPercentage => CountSpawnMonsters / (float)CountMonsters;

    public void Awake()
    {
        if ((Instance ??= this) != this)
            Destroy(gameObject);
    }


    void Start()
    {
        _level = GetComponent<LevelHolder>().level;

        OnStartWave += OnStartWaveMethod;

        CountMonsters = _level
            .waves
            .SelectMany(w => w.subWaves)
            .Sum(sw => sw.count);

        CountWaves = _level.waves.Length;

        var a = StartCoroutine(Spawn());
    }

    private void OnStartWaveMethod(float seconds)
    {
        Debug.Log(nameof(OnStartWaveMethod));

    }

    public IEnumerator Spawn()
    {
        foreach (var wave in _level.waves)
        {
            OnStartWave?.Invoke(wave.pause);
            CurrentPositionWave++;
            yield return new WaitForSeconds(wave.pause);
            foreach (var subWave in wave.subWaves)
            {
                for (var count = 0; count < subWave.count; count++)
                {
                    CountSpawnMonsters++;
                    SpawnMonster(subWave);
                    yield return new WaitForSeconds(wave.spawnTime);
                }
            }
            OnEndWave();
        }
    }



    private void OnEndWave()
    {
        Debug.Log(nameof(OnEndWave));
    }

    public event Action<float> OnStartWave;

    private void SpawnMonster(SubWave subWave)
    {
        var instantiateMonster = Instantiate(subWave.monster, spawnEnemy);
        instantiateMonster.GetComponent<EnemyMoving>().Path = path;
        var enemyInfoHolder = instantiateMonster.GetComponent<EnemyInfoHolder>();
        enemyInfoHolder.EnemyInfo = subWave.enemyInfo;
        enemyInfoHolder.spawner = this;

        if ((subWave.enemyInfo.enemyType & EnemyType.Boss) != 0)
        {
            SpawnHealthBar(instantiateMonster);
        }

    }

    private void SpawnHealthBar(GameObject instantiateMonster)
    {
        var healthBarInstantiate = Instantiate(
            HealthBarPrefab,
            instantiateMonster.transform
        );
        
        healthBarInstantiate.transform.localPosition = new Vector3(0, 1);

        var enemyHealth = instantiateMonster.GetComponent<EnemyHealth>();
        healthBarInstantiate.GetComponent<EnemyHealthBar>().Init(enemyHealth);
    }

    public void AddKill()
    {
        if ((++CountKilledMonsters) == CountMonsters)
            OnKillAllMonsters();
    }

    public void OnKillAllMonsters()
    {
        Debug.Log(nameof(OnKillAllMonsters));
    }
}