using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSettings : MonoBehaviour
{
	[SerializeField] private bool _isInspectionPoint;
	[SerializeField] private float _inspectionTime;

	public Vector3 position { get => transform.position; }
	public bool isInspectionPoint { get => _isInspectionPoint; }
	public float inspectionTime { get => _inspectionTime; }
}
