using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(GameItemPerception))]
[RequireComponent(typeof(NavMeshAgent))]
public class GameItemBehaviour : MonoBehaviour
{
	//
	// Other compnents
	private GameItemPerception perception;
	private NavMeshAgent navMeshAgent; 

	//
	// Editor variables
	[SerializeField] private float randomDecisionRate = 2f;
	[SerializeField] private LayerMask interactWithLayers;

	//
	// Public variables

	//
	// Private variables
	private GameItemState state;
	private float nextRandomDecision;

	//--------------------------
	// MonoBehaviour methods
	//--------------------------
	void Awake()
	{
		perception = GetComponent<GameItemPerception>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	void Start()
	{
		state = new GameItemState();
		nextRandomDecision = Time.time;
	}

	void Update()
	{
		// switching states
		if (Time.time > nextRandomDecision)
		{
			int rand = (int) Mathf.Floor(Random.Range(0, 4));
			//Debug.Log(rand);
			switch (rand)
			{
				case 0:
					state = new GameItemState();
					break;
				case 1:
					state = new GameItemLookAtNearestPlayer(transform, perception.GetNearestPlayer().transform);
					break;
				case 2:
					navMeshAgent.SetDestination(transform.position + new Vector3(Random.Range(-2, 5), 0 , Random.Range(-2, 5)));
					break;
				case 3:
					state = new GameItemTryFollowNearestPlayer(navMeshAgent, perception.GetNearestPlayer().transform, interactWithLayers);
					break;
			}

			nextRandomDecision = Time.time + randomDecisionRate;
		}

		// performing actions
		state.Update();
	}

	//--------------------------
	// GameItemBehaviour methods
	//--------------------------
	public void Kick(Vector3 direction)
	{

	}

	//--------------------------
	// GameItemState classes
	//--------------------------
	private class GameItemState
	{
		public virtual void Update() {}
	}

	private class GameItemLookAtNearestPlayer : GameItemState
	{
		Transform transform;
		Transform targetPlayerTransform;
		
		public GameItemLookAtNearestPlayer(Transform transform, Transform targetPlayerTransform)
		{
			this.transform = transform;
			this.targetPlayerTransform = targetPlayerTransform;
		}

		public override void Update()
		{
			Vector3 targetDirection = targetPlayerTransform.position - transform.position;
			Vector3 direction = Vector3.RotateTowards(transform.forward, targetDirection, 5f * Time.deltaTime, 0f);
			transform.rotation = Quaternion.LookRotation(direction);
		}
	}

	private class GameItemTryFollowNearestPlayer : GameItemState
	{
		NavMeshAgent navMeshAgent;
		Transform targetPlayerTransform;
		LayerMask interactWithLayers;

		public GameItemTryFollowNearestPlayer(NavMeshAgent navMeshAgent, Transform targetPlayerTransform, LayerMask interactWithLayers)
		{
			this.navMeshAgent = navMeshAgent;
			this.targetPlayerTransform = targetPlayerTransform;
			this.interactWithLayers = interactWithLayers;
		}

		public override void Update()
		{
			if (Vector3.Distance(navMeshAgent.transform.position, targetPlayerTransform.position) < 10f)
			{
				navMeshAgent.SetDestination(targetPlayerTransform.position);
			}
		}
	}
}
