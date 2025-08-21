using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkDataSo", menuName = "Scriptable Objects/ChunkDataSo")]
public class ChunkDataSo : ScriptableObject
{
    public List<GameObject> chunkPrefabs;
}
