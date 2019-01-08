using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Game/Data/Weapons"))]
public class WeaponData : ItemsData
{
    public enum WeaponType { Pistol, Rifle, Shotgun };

    [SerializeField] float shotCooldown;
    [SerializeField] float reloadTime;
    [SerializeField] int chargerCapacity;
    [SerializeField] WeaponType weaponType;
    [SerializeField] AudioClip reloadSound;
    [SerializeField] AudioClip[] gunShotSounds;
}
