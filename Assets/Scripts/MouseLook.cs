using System.Collections;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	[SerializeField] private Transform _orientationHelper;
	[SerializeField][Range(50, 200)] private float _sensitivity = 50f;

	private float _xRotation;
	private float _yRotation;

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
	}
	private void LookAround(float mouseX, float mouseY)
	{
		_yRotation += mouseX;
		_xRotation -= mouseY;

		_xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

		transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
		_orientationHelper.rotation = Quaternion.Euler(0, _yRotation, 0);

		transform.position = _orientationHelper.position;
	}
}