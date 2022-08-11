using UnityEngine;

public class JumpingState : State
{
	public JumpingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		Jump();
		stateMachine.ChangeState(character.air);
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
	private void Jump()
	{
		character.transform.Translate(Vector3.up * (character.ColisionRadius + 0.01f));
		character.GetComponent<Rigidbody>().AddForce(character.transform.up * character.JumpForce, ForceMode.Impulse);
	}
}