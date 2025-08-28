using System;
using System.Collections.Generic;
using Abstract;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelGenerator : LevelBasedMonoBehaviour
{
    public static LevelGenerator instance;

    [Header("Level Config So")] [Header("Prefabs")] [SerializeField]
    private GameObject finishChunkPrefab;

    [SerializeField] private GameObject[] chunkPrefabs;

    // [SerializeField] GameObject doorsChunkPrefab;
    [SerializeField] Transform chunkParent;

    [Header("Runtime Data")] [SerializeField]
    private List<GameObject> chunks = new List<GameObject>();

    private int _amountOfChunksSpawned = 0;
    private bool _canMoveChunks = false;

    // [SerializeField] int startingChunkCount = 12;

    [Header("Move Speed Settings")] [SerializeField]
    private float currentMoveSpeed = 5f;

    [SerializeField] private float chunkLength;

    [Header("Spawn Settings")]
    // [SerializeField]
    // private int doorsSpawnerIntMin = 6;

    // [SerializeField] private int doorsSpawnerIntMax = 12;
    [SerializeField]
    private int finishChunkInt = 20;


    public bool IsFinishSpawned => finishLine != null;
    [SerializeField] private GameObject finishLine;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChange;
    }

    void Start()
    {
        InitializeLevelSettings();
        currentMoveSpeed = currentLevel.baseMoveSpeed;
        ScoreManager.instance.SetMultiplier(currentLevel.scoreMultiplier);
        SpawnStartingChunks();
    }


    void Update()
    {
        // if (_canMoveChunks && currentLevel.isMovingChunks)
        //     MoveChunks();
    }


    private void HandleGameStateChange(GameManager.GameState newState)
    {
        _canMoveChunks = newState == GameManager.GameState.Game;
    }

    private void SpawnStartingChunks()
    {
        for (int i = 0; i < currentLevel.startingChunkCount; i++)
        {
            SpawnSingleChunk();
        }
    }

    private void SpawnSingleChunk()
    {
        if (_amountOfChunksSpawned >= currentLevel.chunkCount) return;

        var spawnPositionZ = transform.position.z + 200f;
        // var spawnPositionZ = CalculateSpawnPositionZ();
        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);

        var chunkToSpawn = ChooseChunkToSpawn();
        var newChunkGo = Instantiate(chunkToSpawn, chunkSpawnPos, Quaternion.identity, chunkParent);
        chunks.Add(newChunkGo);

        _amountOfChunksSpawned++;

        ScoreManager.instance.AddScore(10);
    }

    private GameObject ChooseChunkToSpawn()
    {
        if (_amountOfChunksSpawned == currentLevel.chunkCount - 1)
        {
            return currentLevel.finishChunkPrefab;
        }

        var avaliableChunks = currentLevel.avaliableChunks;

        var chunkToSpawn = chunkPrefabs[Random.Range(0, avaliableChunks.Count)];
        return chunkToSpawn;
    }

    private float CalculateSpawnPositionZ()
    {
        return chunks.Count == 0
            ? transform.position.z
            : chunks[^1].transform.position.z + chunkLength;
    }
    
    //disabled moving chunks 
    // private void MoveChunks()
    // {
    //     // disabled moving chunks for now
    //     // currentMoveSpeed += Time.deltaTime * 0.01f;
    //     Vector3 moveDirection = Vector3.forward * (currentMoveSpeed * Time.deltaTime);
    //     for (int i = 0; i < chunks.Count; i++)
    //     {
    //         var chunk = chunks[i];
    //         chunks[i].transform.Translate(0, 0, -moveDirection.z);
    //         if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
    //         {
    //             //TODO multiply with chunk length *2 becasue nemys could be falll
    //
    //             chunks.RemoveAt(i);
    //             Destroy(chunk);
    //             SpawnSingleChunk();
    //         }
    //     }
    // }

    #region Calculating Level Complete Progress

    public float GetFinishZ()
    {
        if (finishLine != null)
            return finishLine.transform.position.z - chunkLength / 2;

        return 0f;
    }

    public float EstimateFinishZ()
    {
        return transform.position.z + chunkLength * (finishChunkInt - 1);
    }

    private float _initialFinishDistance;

    public float GetMoveSpeed()
    {
        return currentMoveSpeed;
    }

    #endregion


  
    public float GetChunkLength()
    {
        return chunkLength;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
    }
}