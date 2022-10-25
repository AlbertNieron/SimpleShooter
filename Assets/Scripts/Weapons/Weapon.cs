using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField] private WeaponSpecifications _specs;
	[SerializeField] private GameObject _projectilePrefab;
	[SerializeField] private Transform _gunBarrel;

	private float _bulletsLeft;
	private bool _readyToShoot = true;
	static private Transform _projectileBin;

	public WeaponSpecifications Specs => _specs;

	private void Awake()
	{
		name = Specs.gunName;
		if (_projectileBin == null)
			_projectileBin = new GameObject("Bullets").transform;
	}

	public void PullTheTrigger()
	{
		print("pull");
		if (_readyToShoot)
		{
			Fire();
			print("fire");
		}
	}
	private void Fire()
	{
		if (_specs.isAutomatic)
		{
			GameObject bullet = Instantiate(_projectilePrefab, _gunBarrel.position, Quaternion.identity);
			bullet.transform.forward = _gunBarrel.forward;
			bullet.transform.parent = _projectileBin.transform;

			Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
			bulletRB.AddForce(Camera.main.transform.forward * _specs.bulletSpeed, ForceMode.Impulse);

			_readyToShoot = false;

			Invoke(nameof(ResetShoot), Specs.timeBetweenShots);
		}
	}

	public void Reload()
	{

	}

	private void ResetShoot()
	{
		_readyToShoot = true;
	}
}
