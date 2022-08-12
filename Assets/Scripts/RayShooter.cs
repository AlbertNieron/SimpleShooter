using System.Collections;
using TMPro;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
	[SerializeField] TMP_Text _debugText;
	private Camera _camera;
	private bool _shot;
	[SerializeField] private float _force;

	private void Start()
	{
		_camera = GetComponent<Camera>();
	}
	private void Update()
	{
		_shot = Input.GetMouseButtonDown(0);

		if (_shot)
		{
			Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
			Ray ray = _camera.ScreenPointToRay(point);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.CompareTag("Movable"))
				{
					hit.rigidbody.AddForce(_camera.transform.forward * _force, ForceMode.Impulse);
				}
				else
				{
					StartCoroutine(SphereIndicator(hit.point));
				}
				_debugText.text = hit.collider.name;
			}
		}
	}
	private IEnumerator SphereIndicator(Vector3 position)
	{
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = position;
		sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

		yield return new WaitForSeconds(2);

		Destroy(sphere);
	}
	private void OnGUI()
	{
		int size = 22;
		float positionX = _camera.pixelWidth / 2 - size / 4;
		float positionY = _camera.pixelHeight / 2 - size / 2;
		GUI.Label(new Rect(positionX, positionY, size, size), "*");
	}
}
