using UnityEngine;

public class GroundedState : State
{
	private float _verticalInput;
	private float _horizontalInput;

	protected float speed;
	protected bool grounded;
	public GroundedState(Character character, StateMachine stateMachine) : base(character, stateMachine)
	{
	}
	public override void Enter()
	{
		base.Enter();
		character.GetComponent<Rigidbody>().drag = character.GroundDrag;
	}
	public override void Exit()
	{
		base.Exit();
	}
	public override void PlayerInput()
	{
		base.PlayerInput();
		_verticalInput = Input.GetAxisRaw("Vertical");
		_horizontalInput = Input.GetAxisRaw("Horizontal");
	}
	public override void LogicUpdate()
	{
		base.LogicUpdate();
	}
	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();
		character.Move(_horizontalInput, _verticalInput, speed);

		grounded = character.CheckCollision(character.transform.position + new Vector3(0, character.ColisionRadius - 0.1f, 0));
	}
}
