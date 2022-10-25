using UnityEngine;
public class StandingState : MovementState  // INHERITANCE
{
	private bool _jump;
	protected bool sprint;
	public StandingState(Player character, StateMachine stateMachine) : base(character, stateMachine)
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
		sprint = Input.GetKey(KeyCode.LeftShift);
	}
	public override void LogicUpdate()  // POLYMORPHISM
	{
		base.LogicUpdate();
		if (_jump && grounded)
			stateMachine.ChangeState(character.jumping);
		if (!grounded)
			stateMachine.ChangeState(character.air);
		if (sprint && grounded)
			stateMachine.ChangeState(character.sprinting);
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
	}
}