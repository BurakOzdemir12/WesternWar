using System;
using System.Collections;
using Abstract;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : LevelBasedMonoBehaviour
{
    private bool _canSpawnEnemy;
    [SerializeField] private Transform enemyParent;
    private float timePassed;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }


    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        _canSpawnEnemy = newState == GameManager.GameState.Game;

        if (!_canSpawnEnemy) return;

        StartCoroutine(SpawnEnemiesCoroutine());
    }

    void Start()
    {
        InitializeLevelSettings();
    }


    void Update()
    {
        timePassed = Time.time;
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        foreach (var wave in currentLevel.enemySpawns)
        {
            int totalSpawned = 0;

            if (wave.enemyCounts.Count != wave.enemyPrefab.Count)
            {
                Debug.LogError("EnemyWaveData: enemyCounts and enemyPrefab list sizes do not match!");
                yield break;
            }

            for (int i = 0; i < wave.enemyPrefab.Count; i++)
            {
                GameObject prefab = wave.enemyPrefab[i];
                int count = wave.enemyCounts[i];

                for (var j = 0; j < count; j++)
                {
                    if (totalSpawned >= wave.maxEnemies)
                        yield break;

                    var randomX = Random.Range(-4f, 4f);
                    var randomZ = Random.Range(wave.spawnOffsetZMin, wave.spawnOffsetZMax);
                    var spawnPosition = new Vector3(randomX, 1f, randomZ);

                    Instantiate(prefab, spawnPosition, Quaternion.identity, enemyParent);

                    totalSpawned++;

                    if (wave.spawnInstant) continue;
                    if (wave.spawnInterval > 0f)
                        yield return new WaitForSeconds(wave.spawnInterval);
                }
            }
        }
    }
    //if you want to use the old coroutine,
    //it isn't have enemy count control for eaach prefab SIMPLE
    // private IEnumerator SpawnEnemiesCoroutine()
    // {
    //     foreach (var wave in currentLevel.enemySpawns)
    //     {
    //         for (int j = 0; j < wave.maxEnemies; j++)
    //         {
    //             float randomZ = Random.Range(wave.spawnOffsetZMin, wave.spawnOffsetZMax);
    //             float randomX = Random.Range(-6f, 6f);
    //             Vector3 spawnPosition = new Vector3(randomX,
    //                 1f,
    //                 randomZ
    //             );
    //             foreach (var enemy in wave.enemyPrefab)
    //             {
    //                 Instantiate(enemy,
    //                     spawnPosition, Quaternion.identity, enemyParent);
    //             }
    //
    //             yield return new WaitForSeconds(wave.spawnInterval);
    //         }
    //     }
    // }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }
}