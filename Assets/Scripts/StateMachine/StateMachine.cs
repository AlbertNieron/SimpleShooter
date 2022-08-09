public class StateMachine
{
	public State CurrentState { get; private set; }
	public void Initialize(State startingState)
	{
		CurrentState = startingState;
		startingState.Enter();
	}
	public void ChangeState(State nextState)
	{
		CurrentState.Exit();
		CurrentState = nextState;
		nextState.Enter();
	}
}
