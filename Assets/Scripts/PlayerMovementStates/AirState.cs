using UnityEngine;

public class AirState : MovementState
{
	private float airTime;
	public AirState(Player character, StateMachine stateMachine) : base(character, stateMachine)
	{

	}
	public override void Enter()
	{
		base.Enter();
		airTime = 1;
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
		airTime += Time.deltaTime;
		if (grounded) stateMachine.ChangeState(character.standing);
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		character.Move(airTime);
	}
}