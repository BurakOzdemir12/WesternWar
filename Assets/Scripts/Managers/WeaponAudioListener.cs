using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAudioListener : MonoBehaviour
{
    public static WeaponAudioListener Instance;
    [SerializeField] private List<WeaponsSo> currentModelsGunsSo;
    [SerializeField] private List<AudioClip> currentGunSfx;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        var weapon = CrowdSystem.Instance.GetStartingWeapon();

        currentModelsGunsSo.Add(weapon);
        if (weapon is WeaponsSo weaponSo)
        {
            currentGunSfx.Clear();
            currentGunSfx.Add(weaponSo.shotSfx);
        }
    }

    private void OnEnable()
    {
        CrowdSystem.OnWeaponChanged += HandleWeaponChanged;
    }

    private void HandleWeaponChanged(WeaponsSo so)
    {
        currentModelsGunsSo.Clear();
        var weapon = CrowdSystem.Instance.GetCurrentWeapon();
        currentModelsGunsSo.Add(weapon);
        currentGunSfx.Clear();

        if (!so && !so.shotSfx) return;

        for (int i = 0; i < currentModelsGunsSo.Count; i++)
        {
            currentGunSfx.Add(weapon.shotSfx);
        }
    }

    public void PlayShotSound()
    {
        if (currentGunSfx.Count <= 0 || !audioSource) return;

        foreach (var clip in currentGunSfx)
        {
            audioSource.clip = clip;
        }

        audioSource.Play();
    }

    private void OnDisable()
    {
        CrowdSystem.OnWeaponChanged -= HandleWeaponChanged;
    }
}