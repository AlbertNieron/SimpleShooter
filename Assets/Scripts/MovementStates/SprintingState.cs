using UnityEngine;
public class SprintingState : StandingState
{
	public SprintingState(Character character, StateMachine stateMachine): base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		speed = character.SprintingSpeed;
	}
	public override void PlayerInput()
	{
		base.PlayerInput();
	}
	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (!sprint)
			stateMachine.ChangeState(character.standing);
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
	public override void Exit()
	{
		base.Exit();
	}
}
