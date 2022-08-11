using UnityEngine;

public class AirState : GroundedState
{
	public AirState(Character character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		speed = character.AirSpeed;
		character.GetComponent<Rigidbody>().drag = character.AirDrag;
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
		if (grounded) stateMachine.ChangeState(character.standing);
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}