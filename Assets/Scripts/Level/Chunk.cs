using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [Header("Prefabs")] [SerializeField] private GameObject doorPrefab;
    [SerializeField] private GameObject barrelPrefab;

    [Header("Spawn Settings")] [SerializeField]
    private float[] lanes = { -6f, 0f, 6f };

    [SerializeField] private int freeFirstChunks = 4;

    [SerializeField] private int maxDoorCount = 3;

    [SerializeField] private int difficultyLevel = 0;

    [SerializeField] private float doorSpawnChance = 0.7f;
    [SerializeField] private float doorOffsetY = 1.0f;

    [SerializeField] private int maxBarrelCount = 3;
    [SerializeField] private float barrelSpawnChance = 0.8f;
    [SerializeField] private float barrelOffsetY = 0.5f;
    [SerializeField] private float barrelOffsetZ = 2f;

    private List<int> availableLanes = new List<int> { 0, 1, 2 };

    private void Start()
    {
        int myIndex = transform.GetSiblingIndex();
        if (myIndex < freeFirstChunks) return;
        // SpawnDoors();
        // SpawnBarrels();
    }

    private void SpawnDoors()
    {
        if (doorPrefab == null) return;

        int spawnCount = Random.Range(1, Mathf.Min(maxDoorCount + 1, availableLanes.Count + 1));
        // int spawnCount = Random.Range(0, lanes.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            if (availableLanes.Count <= 0) break;
            if (Random.value > doorSpawnChance) continue;
            //TODO: Randomize door spawn chance based on difficulty level

            int laneIndex = SelectRandomLane();
            float x = lanes[laneIndex];
            // Vector3 spawnPos = new Vector3(x, transform.position.y + doorOffsetY, transform.position.z);
            // int selectedDoorIndex = Random.Range(0, doorPrefabs.Length);

            // GameObject newDoor = Instantiate(doorPrefab, spawnPos, Quaternion.identity, transform);
            Vector3 spawnLocalPos = new Vector3(x, doorOffsetY, 0f);
            GameObject newDoor = Instantiate(doorPrefab, transform);
            newDoor.transform.localPosition = spawnLocalPos;

            Doors doorsScript = newDoor.GetComponent<Doors>();
            if (doorsScript != null)
            {
                int crowdSize = CrowdSystem.Instance.GetRunnerCount();
                // int crowdSize = FindObjectOfType<CrowdSystem>().transform.childCount;
                doorsScript.SetPenaltyByCrowdSize(crowdSize);
            }
        }
    }

    private void SpawnBarrels()
    {
        if (barrelPrefab == null) return;

        // Her barrel aralıklı z mesafesiyle sıralanabilir
        int spawnCount = Random.Range(1, Mathf.Min(maxBarrelCount + 1, availableLanes.Count + 1));

        for (int i = 0; i < spawnCount; i++)
        {
            if (availableLanes.Count <= 0) break;
            if (Random.value > barrelSpawnChance) continue;

            int laneIndex = SelectRandomLane();
            float x = lanes[laneIndex];
            float z = transform.position.z + i * barrelOffsetZ;
            Vector3 spawnPos = new Vector3(x, transform.position.y + barrelOffsetY, z);
            Instantiate(barrelPrefab, spawnPos, Quaternion.identity, transform);
        }
    }

    private int SelectRandomLane()
    {
        int randomIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomIndex];
        availableLanes.RemoveAt(randomIndex);
        return selectedLane;
    }
}