using UnityEngine;

public class ChasingState : State
{

	public ChasingState(NPC npc, StateMachine stateMachine) : base(npc, stateMachine) { }

	public override void Enter()
	{
		base.Enter();
		if (Assistant.DebugLog)
			Debug.Log("Тоби пизда");
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
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
