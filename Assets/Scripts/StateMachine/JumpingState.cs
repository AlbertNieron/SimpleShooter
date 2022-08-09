using System.Collections;
using UnityEngine;

public class JumpingState : State
{
	public JumpingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		character.GetComponent<Rigidbody>().AddForce(character.transform.up * character.JumpForce, ForceMode.Impulse);
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
		if (character.CheckCollision(character.transform.position))
			stateMachine.ChangeState(character.standing);
	}
}