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
			GameObject obj = Instantiate(prefab);
			obj.SetActive(false);
			objectPool.Enqueue(obj);
		}

		poolDictionary.Add(tag, objectPool);
	}

	public GameObject SpawnFromPool(string tag)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " Doesn't exist.");
			return null;
		}
		GameObject objectToSpawn = poolDictionary[tag].Dequeue();

		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = Vector3.zero;
		objectToSpawn.transform.rotation = Quaternion.identity;

		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

		if (pooledObj != null)
		{
			pooledObj.OnObjectSpawn();
		}

		poolDictionary[tag].Enqueue(objectToSpawn);

		return objectToSpawn;
	}

	public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
			Debug.LogWarning("Pool with tag " + tag + " Doesn't exist.");
			return null;
		}
		GameObject objectToSpawn = poolDictionary[tag].Dequeue();

		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;

		IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

		if (pooledObj != null)
		{
			pooledObj.OnObjectSpawn();
		}

		poolDictionary[tag].Enqueue(objectToSpawn);

		return objectToSpawn;
	}
}
