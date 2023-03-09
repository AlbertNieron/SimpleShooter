using System.Collections;
using UnityEngine;

public class ServicingState : State
{
	public ServicingState(NPC npc, StateMachine enemyBehaviour) : base(npc, enemyBehaviour) { }

	private bool _isSeeingEnemy = false;
	private float _detectionTimer = 0;

	private int _targetPostNumber;
	private int _workingPostNumber;

	private bool _nextPostChosen;
	private bool _beingOnPost;
	private bool _isTimerRunning;

	private ServicePost[] _servicePosts;

	private bool _isReporting;  //Debug

	#region StateMethods
	public override void Enter()
	{
		base.Enter();

		_servicePosts = npc.ServicePosts;
		_beingOnPost = false;
		_isTimerRunning = false;
		if (!_nextPostChosen) { ChooseNextPost(); }
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (_nextPostChosen) { SetTargetPostToMoving(); }

		if (_isSeeingEnemy)
		{
			if (_detectionTimer <= npc.EnemyDetectionTime)
				_detectionTimer += Time.deltaTime;
			else
			{
				stateMachine.ChangeState(npc.chasingState);
			}
		}
		else
			_detectionTimer = 0;

		if (_beingOnPost)
		{
			switch (_servicePosts[_targetPostNumber].TypeofPoint)
			{
				case PointType.InspectionPoint:
					{
						if (_servicePosts[_targetPostNumber].NeedToBeServicing && !_isTimerRunning)
						{
							_workingPostNumber = _targetPostNumber;

							if (Assistant.DebugLog)
								Debug.Log(npc.name + " прибыл на пост " + _servicePosts[_targetPostNumber].Name + " для наблюдения");

							Assistant.Harry.StartCoroutine(StartWorkTimer(_targetPostNumber));
						}
						else if (_servicePosts[_targetPostNumber].NeedToBeServicing)
						{
							InspectPost();
						}
						else
						{
							if (Assistant.DebugLog)
								Debug.Log(npc.name + " ушел с поста наблюдения");

							goto default;
						}

						break;
					}
				case PointType.WorkPoint:
					{
						goto default;
					}
				default:
					{
						_beingOnPost = false;
						ChooseNextPost();

						if (Assistant.DebugLog)
							Debug.Log(npc.name + " прошел маршрутную точку");

						break;
					}
			}
		}
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		CheckDistance();
		if (npc.Essence == EssenceOfNPC.hostile)
		{
			LookForPlayer();
		}
	}

	public override void Exit()
	{
		base.Exit();

		_isSeeingEnemy = false;
		_detectionTimer = 0;
		_servicePosts[_workingPostNumber].NeedToBeServicing = true;
		npc.AI.SetDestination(npc.transform.position);
	}
	#endregion

	#region Methods
	private void ChooseNextPost()
	{
		if (Random.value < npc.ChanceToReturnToPrevPost) { SetTargetPostNumber(_targetPostNumber - 1); }
		else { SetTargetPostNumber(_targetPostNumber + 1); }
		_nextPostChosen = true;
	}

	private void SetTargetPostNumber(int value)
	{
		int max = npc.ServicePosts.Length - 1;
		if (value > max) { _targetPostNumber = 0; }
		else if (value < 0) { _targetPostNumber = max; }
		else { _targetPostNumber = value; }
	}

	private void SetTargetPostToMoving()
	{
		_nextPostChosen = false;
		npc.AI.SetDestination(_servicePosts[_targetPostNumber].Position);
	}

	private void CheckDistance()
	{
		Vector3 distance = _servicePosts[_targetPostNumber].Position - npc.transform.position;
		_beingOnPost = distance.sqrMagnitude <= npc.PointReachingAccuracy;
	}

	private void LookForPlayer()
	{
		Vector3 direction = Head.Position - npc.Head.position;

		if (Vector3.SqrMagnitude(direction) < npc.SqrVisibilityRange) { return; }

		direction = direction.normalized;

		float angle = Mathf.Acos(Vector3.Dot(npc.transform.forward.normalized, direction)) * Mathf.Rad2Deg;

		if (angle <= npc.ViewingAngle)
		{
			if (Assistant.DebugLog)
				Debug.Log("В зоне видимости");

			if (Physics.Linecast(npc.Head.position, Head.Position, out RaycastHit raycastHit))
				if (raycastHit.transform.CompareTag("Player"))
				{
					_isSeeingEnemy = true;

					if (Assistant.DebugRay)
						Debug.DrawLine(npc.Head.position, Head.Position, Color.cyan);

					if (Assistant.DebugLog)
						Debug.Log("Попался!");
				}
				else
				{
					_isSeeingEnemy = false;
				}
		}
	}

	private void InspectPost()
	{
		if (_isReporting)
			return;
		else
		{
			if (Assistant.DebugLog)
				Debug.Log(npc.name + ": наблюдаю");
			_isReporting = true;
		}
	}
	#endregion

	#region Coroutines
	private IEnumerator StartWorkTimer(int postNumber)
	{
		_isTimerRunning = true;

		if (Assistant.DebugLog)
			Debug.Log(npc.name + " запустил таймер работы");

		yield return new WaitForSeconds(_servicePosts[_targetPostNumber].HowLongToWork);

		_servicePosts[postNumber].NeedToBeServicing = false;
		_isTimerRunning = false;

		Assistant.Harry.StartCoroutine(Restore(postNumber));

		if (Assistant.DebugLog)
			Debug.Log(npc.name + " остановил таймер работы");
	}

	public IEnumerator Restore(int postNumber)
	{
		if (Assistant.DebugLog)
			Debug.Log("Пункт " + _servicePosts[postNumber].Name + " обслужен");

		yield return new WaitForSeconds(_servicePosts[postNumber].HowLongToWork);

		_isReporting = false;
		_servicePosts[postNumber].NeedToBeServicing = true;

		if (Assistant.DebugLog)
			Debug.Log("Пункт " + _servicePosts[postNumber].Name + " нуждается в обслуживании");
	}
	#endregion
}