using UnityEngine;

public abstract class MovementState : State
{
	protected float _verticalInput;
	protected float _horizontalInput;

	protected float speed;
	protected bool grounded;
	public MovementState(Player character, StateMachine stateMachine) : base(character, stateMachine)
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
		
		grounded = character.CheckCollision(character.transform.position + new Vector3(0, character.GroundDetectionRadius - 0.1f, 0));
		character.Move(_horizontalInput, _verticalInput, speed, grounded);
	}
}
