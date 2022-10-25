using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float _lifeTime = 8f;

	private float _birthTime;

	private void Start()
	{
		_birthTime = Time.time;
	}

	private void Update()
	{
		float time = (Time.time - _birthTime) / _lifeTime;

		if (time > 1)
			Destroy(gameObject);
	}
}
