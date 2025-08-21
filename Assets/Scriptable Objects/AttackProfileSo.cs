using UnityEngine;


public enum AttackCategory
{
    Melee,
    Ranged,
    AreaOfEffect
}

[CreateAssetMenu(fileName = "AttackProfileSo", menuName = "Scriptable Objects/AttackProfileSo")]
public class AttackProfileSo : ScriptableObject
{
    public AttackCategory category;
    public GameObject projectilePrefab; // Ranged ise
    public GameObject hitEffectPrefab; // Melee
    public float damage;
    [Header("SFX")] public AudioClip hitSfx;
    public float volume = 4f;
}