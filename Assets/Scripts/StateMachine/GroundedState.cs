using System.Collections;
using UnityEngine;

public class GroundedState : State
{
	public GroundedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
	}
	public override void Exit()
	{
		base.Exit();
	}
	public override void PlayerInput()
	{
		base.PlayerInput();
	}
	public override void LogicUpdate()
	{
		base.LogicUpdate();
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}
