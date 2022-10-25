using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSpecifications", menuName = "Asset/WeaponSpecifications")]
public class WeaponSpecifications : ScriptableObject
{
	public string gunName;

	public float bulletSpeed;
	public float ammo;
	public float magazineSize;

	public float timeBetweenShots = 0.8f;
	public bool burst;
	public bool isAutomatic;
	public float bulletsPerShot;
}
