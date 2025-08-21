using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/EnemySo")]
public class EnemySo : ScriptableObject
{
    public GameObject prefab;
    public int health;
    public float speed;
    public float attackCooldown;
    
    //Attack properties
    public AttackProfileSo attackProfiles;
    
    // public float detectionRange;

    public GameObject deathSfx;

}