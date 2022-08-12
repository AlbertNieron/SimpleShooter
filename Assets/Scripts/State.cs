using UnityEngine;
public abstract class State
{
	protected Character character;
	protected StateMachine stateMachine;
	protected State(Character character, StateMachine stateMachine)
	{
		this.character = character;
		this.stateMachine = stateMachine;
	}
	public virtual void Enter()
	{
		character.DebugState(this);
	}
	public virtual void PlayerInput()
	{

	}
	public virtual void LogicUpdate()
	{

	}
	public virtual void PhysicsUpdate()
	{
		character.DebugSpeed();
	}
	public virtual void Exit()
	{

	}
}
