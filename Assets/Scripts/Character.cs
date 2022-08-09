using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character : MonoBehaviour
{
	public StateMachine movementStateMachine;
	public State standingState;
	public State crouchingState;
	public State jumpingState;
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
}
