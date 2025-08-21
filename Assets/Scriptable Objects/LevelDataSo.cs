using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/Level Data")]
public class LevelDataSo : ScriptableObject
{
    public int levelNumber;

    public List<LaneSpawnData> laneSpawns;

    public List<EnemyWaveData> enemySpawns;

    [System.Serializable]
    public class LaneSpawnData
    {
        public List<SpawnInfo> spawnSequence;
    }

    [System.Serializable]
    public class SpawnInfo
    {
        public GameObject prefab;
        public float offsetZ;
    }

    [System.Serializable]
    public class EnemyWaveData
    {
        public List<GameObject> enemyPrefab;
        public List<int> enemyCounts;
        public float spawnInterval;
        public float spawnExactTime;
        public bool spawnInstant;
        public int maxEnemies;
        public float spawnOffsetZMin;
        public float spawnOffsetZMax;
        
    }

    public int chunkCount;
    public int startingChunkCount = 12;
    public List<ChunkDataSo> avaliableChunks = new List<ChunkDataSo>();
    public GameObject finishChunkPrefab;

    public float baseMoveSpeed = 5f;
    public float scoreMultiplier = 1f;
    public bool canPlayerMoveForward = false;
    public bool isMovingChunks = false;
}