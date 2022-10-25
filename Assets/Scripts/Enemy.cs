using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
	[SerializeField] private Weapon[] _weapons = new Weapon[2];
	[SerializeField] private float _speed;
	[SerializeField] private float _viewAngle;
	[SerializeField] private float _health;
	[SerializeField] private float _timeBetweenAttacking;
	[SerializeField] private byte _importance;

	[SerializeField] private GameObject _route;

	private Animator _enemyBehaviour;

	public static NavMeshAgent _enemy { get; private set; }
	public static GuardPosition[] _guardPositions { get; private set; }

	private void Awake()
	{
		_enemy = GetComponent<NavMeshAgent>();
		_enemy.isStopped = true;
		InitializeGuardRoute();
		_enemy.Warp(_guardPositions[Random.Range(0, _guardPositions.Length)].position);
		_enemyBehaviour = GetComponent<Animator>();
		_enemy.isStopped = false;
	}

	private void InitializeGuardRoute()
	{
		int childCount = _route.transform.childCount;
		_guardPositions = new GuardPosition[childCount];

		PointSettings routePoint;
		Vector3 position;
		bool isInspectionPoint;
		float inspectionTime;

		for (int i = 0; i < childCount; i++)
		{
			routePoint = _route.transform.GetChild(i).GetComponent<PointSettings>();
			position = routePoint.position;
			isInspectionPoint = routePoint.isInspectionPoint;
			inspectionTime = routePoint.inspectionTime;
			_guardPositions[i] = new GuardPosition(position, isInspectionPoint, inspectionTime);
		}
		Destroy(_route);
	}
}

public struct GuardPosition
{
	public Vector3 position { get; private set; }
	public bool isInspectionPoint { get; private set; }
	public float timeToInspetion { get; private set; }

	public GuardPosition(Vector3 position, bool isInspectionPoint, float timeToInspetion)
	{
		this.position = position;
		this.isInspectionPoint = isInspectionPoint;
		this.timeToInspetion = timeToInspetion;
	}
}
