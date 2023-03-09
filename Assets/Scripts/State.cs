using UnityEngine;

public abstract class State
{
	protected Player character;
	protected NPC npc;
	protected StateMachine stateMachine;
	protected State(Player character, StateMachine stateMachine)
	{
		this.character = character;
		this.stateMachine = stateMachine;
	}
	protected State(NPC character, StateMachine stateMachine)
	{
		npc = character;
		this.stateMachine = stateMachine;
	}
	public virtual void Enter()
	{
		if (character is not null)
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
		if (character is not null)
			character.DebugSpeed();
		if (npc is not null)
		{
			if (Assistant.DebugRay)
				Debug.DrawRay(npc.Head.position, npc.Head.forward * 10, Color.magenta);
		}
	}
	public virtual void Exit()
	{

	}
}
