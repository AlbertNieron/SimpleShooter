using System.Collections;
using TMPro;
using UnityEngine;

public class Hands : MonoBehaviour
{
	[SerializeField] private Transform _rightHand;
	[SerializeField] private Transform _pocket;
	[SerializeField] private Transform _itemsOnTheGroundFolder;
	[SerializeField] private Transform _dropDirection;

	[SerializeField] private int _numberOfWeapons = 2;
	[SerializeField] private float _dropForce = 4;

	[Header("Animation timings")]
	[SerializeField] private float _liftItemDuration = .5f;
	[SerializeField] private float _changingWeaponDuration = .5f;

	[Header("Debug")]
	[SerializeField] private TMP_Text _currentSlotDebug;

	private Weapon[] _weaponSlots;
	private int _currentWeaponSlot = 0;

	private int CurrentWeaponSlot
	{
		get { return _currentWeaponSlot; }
		set
		{
			int previousSlot = _currentWeaponSlot;

			if (value >= _weaponSlots.Length)
			{
				_currentWeaponSlot = 0;
			}
			else
			{
				_currentWeaponSlot = value;
			}

			SetWeapon(previousSlot, _currentWeaponSlot);

			_currentSlotDebug.text = "Current weapon slot: " + _currentWeaponSlot.ToString();
		}
	}

	public static bool IsItemMooving { get; private set; }

	public Weapon CurrentWeapon
	{
		get { return _weaponSlots[CurrentWeaponSlot]; }
	}

	private void Awake()
	{
		_weaponSlots = new Weapon[_numberOfWeapons];
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F) && Head.CanPickUp)
		{
			TryPickUp(Head.TargetItem);
		}

		if (Input.GetKeyDown(KeyCode.X) && CurrentWeapon != null)
		{
			DropItem(CurrentWeapon.gameObject);
		}

		if (Input.GetKeyDown(KeyCode.Q) && !IsItemMooving)
		{
			NextWeapon();
		}

		if (Input.GetKey(KeyCode.Mouse0) && !IsItemMooving && CurrentWeapon != null)
		{
			CurrentWeapon.PullTheTrigger();
		}
	}

	private void SetWeapon(int previousSlot, int slot)
	{
		if (_weaponSlots[previousSlot] is not null)
			StartCoroutine(MoveItem(_weaponSlots[previousSlot].gameObject, _pocket, _changingWeaponDuration, false, false));

		if (_weaponSlots[slot] is not null)
			StartCoroutine(MoveItem(_weaponSlots[slot].gameObject, _rightHand, _changingWeaponDuration, false));
	}

	private void NextWeapon()
	{
		CurrentWeaponSlot++;
	}

	private int? GiveSlot(string whatIsSlot)
	{
		switch (whatIsSlot)
		{
			case "Weapon":
				{
					for (int i = 0; i < _weaponSlots.Length; i++)
					{
						if (_weaponSlots[i] == null)
						{
							return i;
						}
					}
					return CurrentWeaponSlot;
				}
			default: return null;
		}
	}

	private void TryPickUp(GameObject item)
	{
		int? slot = GiveSlot(item.tag);
		if (slot == null) { return; }

		switch (item.tag)
		{
			case "Weapon":
				{
					Weapon targetWeapon = item.GetComponent<Weapon>();

					if (_weaponSlots[(int)slot] != null)
					{
						DropItem(CurrentWeapon.gameObject);
					}

					int installationSlot = CurrentWeapon == null ? CurrentWeaponSlot : (int)slot;

					StartCoroutine(MoveItem(item, _rightHand, _liftItemDuration, true, CurrentWeaponSlot == installationSlot));

					_weaponSlots[installationSlot] = targetWeapon;
					return;
				}
			default: return;
		}
	}

	IEnumerator MoveItem(GameObject item, Transform destination, float duration = 0, bool usePhysics = false, bool setActive = true)
	{
		if (IsItemMooving) { yield return null; }

		IsItemMooving = true;

		if (usePhysics && item.TryGetComponent(out Rigidbody itemRb))
		{
			SwitchRigidBody(itemRb);
		}

		Vector3 startPosition = item.transform.position;
		Quaternion startRotation = item.transform.rotation;
		if (duration > 0)
		{
			duration = Mathf.Abs(duration);
			float time = 0;
			while (time < 1f)
			{
				float squaredTime = time * time;
				item.transform.SetPositionAndRotation(Vector3.Lerp(startPosition, destination.position, squaredTime), Quaternion.Lerp(startRotation, destination.rotation, squaredTime));

				time += Time.deltaTime / duration;
				yield return null;
			}
		}
		item.transform.SetPositionAndRotation(destination.position, destination.rotation);
		item.transform.SetParent(destination);
		item.SetActive(setActive);
		IsItemMooving = false;
	}

	private void DropItem(GameObject item)
	{
		item.transform.SetParent(_itemsOnTheGroundFolder);
		if (item.TryGetComponent(out Rigidbody itemRb))
		{
			SwitchRigidBody(itemRb);
		}
		else
		{
			item.AddComponent<Rigidbody>();
			item.AddComponent<DeleteRigidBodyAfterStop>();
		}

		itemRb.AddForce(_dropDirection.forward * _dropForce, ForceMode.Impulse);
		switch (item.tag)
		{
			case "Weapon":
				{
					_weaponSlots[CurrentWeaponSlot] = null;
					return;
				}
		}
	}
	private void SwitchRigidBody(Rigidbody target)
	{
		target.isKinematic = !target.isKinematic;
		target.detectCollisions = !target.detectCollisions;
	}
}
