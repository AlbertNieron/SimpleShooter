using TMPro;
using UnityEngine;

public class Head : MonoBehaviour
{
	[SerializeField] private Camera _eyes;
	[SerializeField][Range(10, 200)] private float _sensitivity = 50f;
	[SerializeField] private LayerMask _playerLayer;

	[SerializeField] private TMP_Text _idPlate;
	[SerializeField] private float _checkDistanceSquared;

	private float _xRotation;
	private float _yRotation;

	[SerializeField]
	static public GameObject TargetItem { get; private set; }
	public static bool CanPickUp { get; private set; }

	private void Awake()
	{
		CanPickUp = false;
		_idPlate.gameObject.SetActive(false);
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		float horizontalInput = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
		float verticalInput = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;

		LookAround(horizontalInput * _sensitivity, verticalInput * _sensitivity);

		if (Hands.isItemMooving)
		{
			CanPickUp = false;
		}
	}

	private void FixedUpdate()
	{
		if (!Hands.isItemMooving)
		{
			CheckObjectOfInterest();
		}
		IdentifyTargetItem();
	}

	private void LookAround(float mouseX, float mouseY)
	{
		_yRotation += mouseX;
		_xRotation -= mouseY;

		_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

		_eyes.transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
		transform.rotation = Quaternion.Euler(0, _yRotation, 0);

		_eyes.transform.position = transform.position;
	}

	private void CheckObjectOfInterest()
	{
		RaycastHit sightPoint;

		if (Physics.Raycast(transform.position, _eyes.transform.forward, out sightPoint,3f, _playerLayer))
		{
			if (sightPoint.transform.GetComponent<CanPickUp>())
			{
				float distance = Vector3.SqrMagnitude(sightPoint.transform.position - transform.position);
				if(distance < _checkDistanceSquared)
				{
					TargetItem = sightPoint.transform.gameObject;
					CanPickUp = true;
				}
			}
			else
			{
				CanPickUp = false;
			}
		}
	}

	private void IdentifyTargetItem()
	{
		if (CanPickUp)
		{
			Vector3 platePos = Camera.main.WorldToScreenPoint(TargetItem.transform.position);
			_idPlate.transform.position = platePos;
			_idPlate.text = TargetItem.tag + ": " +TargetItem.name;
			_idPlate.gameObject.SetActive(true);
		}
		else
			_idPlate.gameObject.SetActive(false);
	}
}