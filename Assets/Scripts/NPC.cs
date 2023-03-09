using UnityEngine;
using UnityEngine.AI;
public enum EssenceOfNPC
{
	peaceful,
	neutral,
	hostile
}

[RequireComponent(typeof(NavMeshAgent))]

public class NPC : MonoBehaviour
{
	#region StateMachine
	internal StateMachine enemyBehaviour;
	internal State servicingState;
	internal State attackingState;
	internal State searchingState;
	internal State chasingState;
	internal State communicationState;
	#endregion

	[SerializeField] private Transform _head;

	[Header("Characteristic")]
	[SerializeField] private EssenceOfNPC _essence;
	[SerializeField] private float _health;
	[SerializeField] private float _sqrVisibilityRange;
	[SerializeField] private float _viewingAngle;

	[Header("Combat")]
	[SerializeField] private Weapon[] _weapons = new Weapon[2];
	[SerializeField] private float _enemyDetectionTime = 2;
	[SerializeField] private float _sqrAttackingRange = 5;
	[SerializeField] private float _timeBetweenAttacking = 2;

	[Header("Working route")]
	[SerializeField] private GameObject _route;
	[SerializeField] private float _chanceToReturnToPreviousServicePost = 0.01f;
	[SerializeField] private float _pointReachingAccuracy;

	public NavMeshAgent AI { get; private set; }
	public ServicePost[] ServicePosts { get; set; }
	public float ChanceToReturnToPrevPost { get => _chanceToReturnToPreviousServicePost; }
	public float PointReachingAccuracy => _pointReachingAccuracy;
	public EssenceOfNPC Essence => _essence;
	public float ViewingAngle => _viewingAngle;
	public float SqrVisibilityRange => _sqrVisibilityRange;
	public Transform Head => _head;

	public float EnemyDetectionTime { get => _enemyDetectionTime; }

	private void Awake()
	{
		enemyBehaviour = new StateMachine();
		servicingState = new ServicingState(this, enemyBehaviour);
		attackingState = new AttackingState(this, enemyBehaviour);
		chasingState = new ChasingState(this, enemyBehaviour);


		AI = GetComponent<NavMeshAgent>();
		InitializeServicingRoute();

		enemyBehaviour.Initialize(servicingState);
	}
	private void Update()
	{
		enemyBehaviour.CurrentState.PlayerInput();
		enemyBehaviour.CurrentState.LogicUpdate();
	}

	private void FixedUpdate()
	{
		enemyBehaviour.CurrentState.PhysicsUpdate();
	}

	private void InitializeServicingRoute()
	{
		int childCount = _route.transform.childCount;
		ServicePosts = new ServicePost[childCount];

		PostSetting routePoint;

		string name;
		Vector3 position;
		PointType typeOfPoint;
		float howLongToWork;

		for (int i = 0; i < childCount; i++)
		{
			routePoint = _route.transform.GetChild(i).GetComponent<PostSetting>();

			name = routePoint.name;
			position = routePoint.Position;
			typeOfPoint = routePoint.TypeOfPoint;
			howLongToWork = routePoint.HowLongToWork;

			ServicePosts[i] = new ServicePost(name, position, typeOfPoint, howLongToWork);
		}
		if (Assistant.DestroyRoutes)
			Destroy(_route);
	}

}