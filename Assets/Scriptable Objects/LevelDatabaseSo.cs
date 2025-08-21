using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Scriptable Objects/Level Database")]
public class LevelDatabaseSo : ScriptableObject
{
    public List<LevelDataSo> levels; 
}
