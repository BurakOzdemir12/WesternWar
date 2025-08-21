using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/PlayerSo")]
public class PlayerSo : ScriptableObject
{
    public GameObject modelPrefab;
    public int health;

    // public float detectionRange;

    // VFX
    public GameObject spawnVFx;
    
    public GameObject hitVFx;

    public GameObject hitSFx;

    //SFX
}