internal class AttackingState : State
{
	public AttackingState(NPC enemy, StateMachine stateMachine) : base(enemy, stateMachine) { }

	public override void Enter()
	{
		base.Enter();
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

	public override void Exit()
	{
		base.Exit();
	}
}