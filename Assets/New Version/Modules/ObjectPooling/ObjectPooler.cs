using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : NetworkBehaviour
{
	//
	// Pool class
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	//
	// Singleton
	private static ObjectPooler instance;
	public static ObjectPooler Instance
	{
		get
		{
			return instance;
		}
	}

	//
	// Public variables
	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;
	public List<GameObject> temp = new List<GameObject>();

	/// <summary>
	/// Ensures singleton functionality and initializes poolDictionary
	/// </summary>
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Only one instance of ObjectPooler is allowed. Destroying the new instance...");
		}
		else
			instance = this;

		poolDictionary = new Dictionary<string, Queue<GameObject>>();
	}

	private void Start()
	{
		if (!isServer)
			return;
		StartCoroutine(SpawnCoroutine());
	}

	/// <summary>
	/// Creates a new object pool
	/// </summary>
	/// <param name="tag">Identificator of the pool</param>
	/// <param name="prefab">Prefab the pool is going to create clones of</param>
	/// <param name="size">Amount of clones created on initialiation</param>
	public void CreateNewPool(string tag, GameObject prefab, int size)
	{
		if (!isServer)
			return;

		if (poolDictionary.ContainsKey(tag))
		{
			return;
		}
		Queue<GameObject> objectPool = new Queue<GameObject>();

		for (int i = 0; i < size; i++)
		{
			GameObject obj = Instantiate(prefab, transform);
			temp.Add(obj);
			obj.SetActive(false);
			objectPool.Enqueue(obj);
		}

		poolDictionary.Add(tag, objectPool);
	}

	public IEnumerator SpawnCoroutine()
	{
		yield return new WaitForSeconds(10f);

		Debug.Log("Temp size: " + temp.Count);

		foreach (var item in temp)
		{
			NetworkServer.Spawn(item);
		}
	}

	/// <summary>
	/// Spawns a game object using pool optimization
	/// </summary>
	/// <param name="tag">Identificator of the pool to get the an object from</param>
	/// <returns>The spawned game object</returns>
	public GameObject SpawnFromPool(string tag)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
			return null;
		}

		GameObject objectToSpawn = poolDictionary[tag].Peek();

		// Checking if an object is available to take
		if (!objectToSpawn.gameObject.activeSelf)
		{
			// taking an existing object
			poolDictionary[tag].Dequeue();
			objectToSpawn.transform.position = Vector3.zero;
			objectToSpawn.transform.rotation = Quaternion.identity;
		}
		else
		{
			// creating a new object because all enqueued objects are in use
			objectToSpawn = Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity, transform);
		}

		objectToSpawn.SetActive(true);

		// Do we need this?
		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
		if (pooledObj != null)
		{
			pooledObj.OnObjectSpawn();
		}

		// putting the object in the queue
		poolDictionary[tag].Enqueue(objectToSpawn);

		return objectToSpawn;
	}

	/// <summary>
	/// Spawns a game object using pool optimization
	/// </summary>
	/// <param name="tag">Identificator of the pool to get the an object from</param>
	/// <returns>The spawned game object</returns>
	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
			return null;
		}

		GameObject objectToSpawn = poolDictionary[tag].Peek();

		// Checking if an object is available to take
		if (!objectToSpawn.gameObject.activeSelf)
		{
			// taking an existing object
			poolDictionary[tag].Dequeue();
			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;
		}
		else
		{
			// creating a new object because all enqueued objects are in use
			objectToSpawn = Instantiate(objectToSpawn, position, rotation, transform);
		}		

		objectToSpawn.SetActive(true);

		// Do we need this?
		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
		if (pooledObj != null)
		{
			pooledObj.OnObjectSpawn();
		}

		// putting the object in the queue
		poolDictionary[tag].Enqueue(objectToSpawn);

		return objectToSpawn;
	}
}
