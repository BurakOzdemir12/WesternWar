using System;
using System.Collections;
using System.Collections.Generic;
using Abstract;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaneSpawner : LevelBasedMonoBehaviour
{
    [Header("Level Config So")] [SerializeField]
    private Transform laneParent;

    [SerializeField] private float spawnOffsetY = 0.6f;
    [SerializeField] private float spawnInterval = 2.5f;

    [SerializeField] private List<GameObject> lanes = new List<GameObject>();
    private bool _canMoveLines;

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChange;
    }

    private void Start()
    {
        InitializeLevelSettings();
    }

    private void Update()
    {
        if (_canMoveLines) MoveLines();
    }

    private void HandleGameStateChange(GameManager.GameState newState)
    {
        _canMoveLines = newState == GameManager.GameState.Game;

        StartCoroutine(SpawnLanesCoroutine());
    }


    private IEnumerator SpawnLanesCoroutine()
    {
        for (int i = 0; i < currentLevel.laneSpawns.Count; i++)
        {
            foreach (var spawnInfo in currentLevel.laneSpawns[i].spawnSequence)
            {
                float z = transform.position.z + spawnInfo.offsetZ;
                Vector3 spawnPosition = new Vector3(transform.position.x, spawnOffsetY, z);

                if (spawnInfo.prefab == null)
                {
                    Debug.LogWarning("Lane prefab is null, skipping spawn.");
                    continue;
                }

                var newLaneGo = Instantiate(spawnInfo.prefab, spawnPosition, Quaternion.identity,
                    laneParent);
                lanes.Add(newLaneGo);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void MoveLines()
    {
        Vector3 moveDirection = Vector3.forward * (currentLevel.baseMoveSpeed * Time.deltaTime);

        // Tersten git: hem RemoveAt, hem index hatası yaşamamak için
        for (int i = lanes.Count - 1; i >= 0; i--)
        {
            var lane = lanes[i];
            lane.transform.Translate(0, 0, -moveDirection.z);

            if (lane.transform.position.z <=
                Camera.main.transform.position.z - 20f)
            {
                if (lane != null && lane.scene.IsValid()) // sahne objesi mi?
                {
                    lanes.RemoveAt(i);
                    Destroy(lane);
                }
            }
        }
    }


    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
    }
}