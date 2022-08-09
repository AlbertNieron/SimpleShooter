using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character : MonoBehaviour
{
	#region StateMachineVariables
	public StateMachine movementStateMachine;
	public State standingState;
	public State crouchingState;
	public State jumpingState;
	#endregion
	#region Variables
	[SerializeField] Transform _orientationHelper;

	public float Speed = 2;
	#endregion
	private void Start()
	{
		movementStateMachine = new StateMachine();
		standingState = new StandingState(this, movementStateMachine);
		crouchingState = new CrouchingState(this, movementStateMachine);
		jumpingState = new JumpingState(this, movementStateMachine);

		movementStateMachine.Initialize(standingState);
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

	public void Move(float horizontalSpeed, float verticalSpeed)
	{
		Vector3 targetVelocity = _orientationHelper.forward * verticalSpeed + _orientationHelper.right * horizontalSpeed;
		GetComponent<Rigidbody>().AddForce(targetVelocity * Time.deltaTime, ForceMode.Force);
	}
}
