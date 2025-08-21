using UnityEngine;

[CreateAssetMenu(fileName = "Weapons", menuName = "Scriptable Objects/WeaponsSo")]
public class WeaponsSo : ScriptableObject
{
    public float range;
    public float fireRate;
    public float damage;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public GameObject weaponModel;

    //Sfx
    public AudioClip shotSfx;
    //Vfx
    public ParticleSystem muzzleVfx;
    public GameObject bulletVfx;
}