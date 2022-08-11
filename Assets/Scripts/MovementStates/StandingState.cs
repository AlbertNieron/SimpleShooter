using UnityEngine;
public class StandingState : GroundedState  // INHERITANCE
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
	public override void LogicUpdate()  // POLYMORPHISM
	{
		base.LogicUpdate();
		if (_jump && grounded)
			stateMachine.ChangeState(character.jumping);
		if (!grounded)
			stateMachine.ChangeState(character.air);
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}