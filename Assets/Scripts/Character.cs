using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
	#region StateMachine
	[HideInInspector] public StateMachine movementStateMachine;
	[HideInInspector] public State standing;
	[HideInInspector] public State jumping;
	[HideInInspector] public State air;
	#endregion

	#region Variables
	[SerializeField] private Transform _orientationHelper;
	[SerializeField] private LayerMask _groundLayer;
	public TMP_Text currentState;

	[Header("Player characteristic")]
	[SerializeField] private float _speed;
	[SerializeField] private float _airSpeed;
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _collisionRadius;
	[SerializeField] private float _groundDrag;
	[SerializeField] private float _airDrag;
	#endregion

	#region Properties
	public float Speed => _speed;   // ENCAPSULATION
	public float AirSpeed => _airSpeed;
	public float JumpForce => _jumpForce;
	public float ColisionRadius => _collisionRadius;
	public float GroundDrag => _groundDrag;
	public float AirDrag => _airDrag;
	#endregion
	private void Start()
	{
		GetComponent<Rigidbody>().freezeRotation = true;

		movementStateMachine = new StateMachine();
		standing = new StandingState(this, movementStateMachine);
		jumping = new JumpingState(this, movementStateMachine);
		air = new AirState(this, movementStateMachine);

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
	public void Move(float horizontalSpeed, float verticalSpeed, float speed)   // ABSTRACTION
	{
		Vector3 targetDirection = _orientationHelper.forward * verticalSpeed + _orientationHelper.right * horizontalSpeed;
		GetComponent<Rigidbody>().AddForce(targetDirection.normalized * 10f * speed, ForceMode.Force);
	}
	public bool CheckCollision(Vector3 pointToCheck)
	{
		return Physics.CheckSphere(pointToCheck, ColisionRadius, _groundLayer);
	}
}