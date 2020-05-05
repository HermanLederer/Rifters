using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(GameItemPerception))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class GameItemBehaviour : MonoBehaviour
{
	//
	// Other compnents
	private GameItemPerception perception;
	new private SphereCollider collider;
	new private Rigidbody rigidbody;
	private NavMeshAgent navMeshAgent;

	//
	// Editor variables
	[Header("Meshes")]
	[SerializeField] private Mesh ballMesh = null;
	[SerializeField] private Mesh dragonMesh = null;
	[SerializeField] private MeshFilter meshFilter = null;
	[Header("AI")]
	[SerializeField] private float randomDecisionRate = 2f;
	[SerializeField] private LayerMask playerLayer = 0;
	[SerializeField] private LayerMask spellLayer = 0;

	//
	// Public variables

	//
	// Private variables
	public bool isBall { get; private set; }
	private GameItemState state;
	private float nextRandomDecision;
	private float nextDragonTime;

	//
	// Accessors
	public bool isDragon
	{
		get { return !isBall; }
	}

	//--------------------------
	// MonoBehaviour methods
	//--------------------------
	void Awake()
	{
		perception = GetComponent<GameItemPerception>();
		collider = GetComponent<SphereCollider>();
		rigidbody = GetComponent<Rigidbody>();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	void Start()
	{
		BecomeDragon();

		state = new GameItemState();
		nextRandomDecision = Time.time;
	}

	void Update()
	{
		// Modes
		if (Physics.OverlapSphere(transform.position, 6f, playerLayer).Length > 0)
		{
			BecomeBall();
			nextDragonTime = Time.time + 1.4f;
		}
		else
		{
			if (Time.time >= nextDragonTime && rigidbody.velocity.magnitude <= 1f)
			{
				BecomeDragon();
			}
		}

		// States
		if (isDragon && Time.time > nextRandomDecision)
		{
			int rand = (int) Mathf.Floor(Random.Range(0, 4));
			//Debug.Log(rand);
			switch (rand)
			{
				case 0:
					state = new GameItemState();
					break;
				case 1:
					//state = new GameItemLookAtNearestPlayer(transform, perception.GetNearestPlayer().transform);
					break;
				case 2:
					navMeshAgent.SetDestination(transform.position + new Vector3(Random.Range(-2, 5), 0 , Random.Range(-2, 5)));
					break;
				case 3:
					//state = new GameItemTryFollowNearestPlayer(navMeshAgent, perception.GetNearestPlayer().transform, interactWithLayers);
					break;
			}

			nextRandomDecision = Time.time + randomDecisionRate;
		}

		state.Update();
	}

	// TODO: check for that in Update instead
	private void OnTriggerEnter(Collider other)
	{
		if (spellLayer == (spellLayer & 1 << other.gameObject.layer))
		{
			Debug.Log("Entra Proyectil");
			BecomeBall();
			nextDragonTime = Time.time + 1.4f;
		}
	}

	//--------------------------
	// GameItemBehaviour methods
	//--------------------------
	public bool BecomeBall()
	{
		if (isBall) return false;

		// bool
		isBall = true;

		// mesh
		meshFilter.mesh = ballMesh;

		// components
		navMeshAgent.enabled = false;
		collider.enabled = true;
		rigidbody.isKinematic = false;

		return true;
	}

	public bool BecomeDragon()
	{
		if (isDragon) return false;

		// bool
		isBall = false;

		// mesh
		meshFilter.mesh = dragonMesh;

		// components
		Vector3 vel = rigidbody.velocity; // used later to set navmesh destination
		navMeshAgent.enabled = true;
		collider.enabled = false;
		rigidbody.isKinematic = true;

		// rotation and navmesh destination
		transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
		navMeshAgent.SetDestination(transform.position + vel);

		return true;
	}

	public void Kick(Vector3 direction)
	{
		rigidbody.AddForce(direction * rigidbody.mass, ForceMode.Impulse);
	}

	public void SetNextDragonTime(float amount)
	{
		nextDragonTime = Time.time + amount;
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
