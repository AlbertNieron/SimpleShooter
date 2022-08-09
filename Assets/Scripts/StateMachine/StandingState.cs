using System;
using UnityEngine;

public class StandingState : GroundedState
{
	private bool _jump;
	public StandingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		speed = character.Speed;
	}
	public override void Exit()
	{
		base.Exit();
	}
	public override void PlayerInput()
	{
		base.PlayerInput();
		_jump = Input.GetButtonDown("Jump");
	}
	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (_jump)
		{
			if (character.CheckCollision(character.transform.position))
			{
				stateMachine.ChangeState(character.jumping);
			}
		}
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}