using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform muzzleEnd;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource audioSource;

    private WeaponsSo _weaponData;

    public void Initialize(WeaponsSo data)
    {
        _weaponData = data;
        if (_weaponData.shotSfx != null)
            audioSource.clip = _weaponData.shotSfx;
    }

    public void Fire(WeaponsSo currentSo)
    {
        PlayMuzzleFlash();
        WeaponAudioListener.Instance.PlayShotSound();
        // PlayShotSound();
        SpawnBullet();
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();
    }

    private void PlayShotSound()
    {
        if (!audioSource || !_weaponData.shotSfx) return;

        audioSource.PlayOneShot(_weaponData.shotSfx);
    }

    private void SpawnBullet()
    {
        if (_weaponData.bulletPrefab == null) return;

        GameObject bullet = Instantiate(
            _weaponData.bulletPrefab,
            muzzleEnd.position,
            muzzleEnd.rotation
        );

        if (bullet.TryGetComponent(out Rigidbody rb))
            rb.linearVelocity = muzzleEnd.forward * _weaponData.bulletSpeed;
    }
}