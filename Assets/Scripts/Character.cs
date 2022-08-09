using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
	#region StateMachineVariables
	public StateMachine movementStateMachine;
	public State standing;
	public State crouching;
	public State jumping;
	#endregion

	#region Variables
	[SerializeField] private Transform _orientationHelper;

	[SerializeField] private LayerMask _groundLayer;

	public float Speed;
	public float JumpForce;
	#endregion

	#region Properties

	#endregion
	private void Start()
	{
		GetComponent<Rigidbody>().freezeRotation = true;

		movementStateMachine = new StateMachine();
		standing = new StandingState(this, movementStateMachine);
		crouching = new CrouchingState(this, movementStateMachine);
		jumping = new JumpingState(this, movementStateMachine);

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

	public void Move(float horizontalSpeed, float verticalSpeed, float speed)
	{
		Vector3 targetDirection = _orientationHelper.forward * verticalSpeed + _orientationHelper.right * horizontalSpeed;
		GetComponent<Rigidbody>().AddForce(targetDirection.normalized * 10f * speed, ForceMode.Force);
	}
	public bool CheckCollision(Vector3 pointToCheck) => Physics.CheckSphere(pointToCheck, 0.2f, _groundLayer);
}
