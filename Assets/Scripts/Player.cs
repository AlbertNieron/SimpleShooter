using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	#region StateMachine
	internal StateMachine movementStateMachine;
	internal State standing;
	internal State jumping;
	internal State air;
	internal State sprinting;
	#endregion

	#region Variables
	[SerializeField] private Transform _head;
	[SerializeField] private LayerMask _groundLayer;

	[Header("Player characteristic")]
	[SerializeField] private float _speed = 30f;
	[SerializeField] private float _sprintingSpeed = 42f;
	[SerializeField] private float _airSpeed = 4f;
	[SerializeField] private float _jumpForce = 6f;
	[SerializeField] private float _groundDetectionRadius = 0.2f;
	[SerializeField] private float _groundDrag = 5f;
	[SerializeField] private float _airDrag = 0.9f;

	[Header("Debug")]
	[SerializeField] private TMP_Text _debugState;
	[SerializeField] private TMP_Text _debugSpeed;

	internal Rigidbody _playerRB;
	private Vector3 _moveDirection;
	private RaycastHit _slopeHit;
	private float _gravity = 9.81f;
	#endregion

	#region Properties
	public float Speed => _speed;   // ENCAPSULATION
	public float AirSpeed => _airSpeed;
	public float JumpForce => _jumpForce;
	public float GroundDetectionRadius => _groundDetectionRadius;
	public float GroundDrag => _groundDrag;
	public float AirDrag => _airDrag;
	public float SprintingSpeed => _sprintingSpeed;
	#endregion

	#region Foundation
	private void Awake()
	{
		_playerRB = GetComponent<Rigidbody>();
		_playerRB.freezeRotation = true;
		_playerRB.useGravity = false;

		movementStateMachine = new StateMachine();
		standing = new StandingState(this, movementStateMachine);
		jumping = new JumpingState(this, movementStateMachine);
		air = new AirState(this, movementStateMachine);
		sprinting = new SprintingState(this, movementStateMachine);

		movementStateMachine.Initialize(standing);
	}
	private void Update()
	{
		movementStateMachine.CurrentState.PlayerInput();
		movementStateMachine.CurrentState.LogicUpdate();
	}
	private void FixedUpdate()
	{
		movementStateMachine.CurrentState.PhysicsUpdate();
	}
	#endregion

	#region Methods
	public void Move(float horizontalInput, float verticalInput, float speed, bool grounded)    //ABSTRACTION
	{
		_moveDirection = _head.forward * verticalInput + _head.right * horizontalInput;
		if (grounded)
		{
			float angle = CalculateSlopeAngle();
			if (angle > 0)
			{
				_moveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal);
			}
		}
		_playerRB.AddForce(_moveDirection.normalized * _playerRB.mass * speed, ForceMode.Force);
	}
	public void Move(float airTime)
	{
		float squaredAirTime = airTime * airTime;
		_playerRB.AddForce(Vector3.down * _playerRB.mass * squaredAirTime * _gravity, ForceMode.Force);
	}
	public float CalculateSlopeAngle()
	{
		Physics.Raycast(transform.position, Vector3.down, out _slopeHit);
		float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
		return angle;
	}
	public bool CheckCollision(Vector3 pointToCheck)
	{
		return Physics.CheckSphere(pointToCheck, GroundDetectionRadius, _groundLayer);
	}
	#endregion

	#region Debug
	public void DebugState(State state)
	{
		_debugState.text = state.ToString();
	}
	public void DebugSpeed()
	{
		_debugSpeed.text = Mathf.Round(_playerRB.velocity.magnitude).ToString();
	}
	#endregion
}