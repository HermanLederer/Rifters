using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
	[System.Serializable]
	public class Pool
	{
		public string tag;
		public GameObject prefab;
		public int size;
	}

	public static ObjectPooler instance;

	private void Awake()
	{
		instance = this;

		poolDictionary = new Dictionary<string, Queue<GameObject>>();
	}

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;
	void Start()
	{
		foreach (Pool pool in pools)
		{
			CreateNewPool(pool.tag, pool.prefab, pool.size);
		}
	}

	public void CreateNewPool(string tag, GameObject prefab, int size)
	{
		Queue<GameObject> objectPool = new Queue<GameObject>();

		for (int i = 0; i < size; i++)
		{
			GameObject obj = Instantiate(prefab, transform);
			obj.SetActive(false);
			objectPool.Enqueue(obj);
		}

		poolDictionary.Add(tag, objectPool);
	}

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
