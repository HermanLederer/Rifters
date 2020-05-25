using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
	// Singleton
	public static VFXManager instance { get; private set; }

	//
	// Editor variables
	#region Editor variables
	[Header("Pool prefabs")]
	[SerializeField] private GameObject explosionVFXPrefab = null;
	//[SerializeField] private GameObject frostVFXPrefab = null;
	#endregion

	private bool firstMusicSourceIsPlaying;
	private const string explosionVFXPool = "ExplosionVFXPool";
	private const string frostVFXPool = "FrostVFXPool";

	//--------------------------
	// MonoBehaviour methods
	//--------------------------
	private void Awake()
	{
		// singleton
		if (instance != null && instance != this)
		{
			Debug.LogError("Impossible to initiate more than one VFXManager. Destroying the instance...");
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		ObjectPooler.Instance.CreateNewPool(explosionVFXPool, explosionVFXPrefab, 10);
		//ObjectPooler.Instance.CreateNewPool(frostVFXPool, frostVFXPrefab, 10);
	}

	//--------------------------
	// VFXManager methods
	//--------------------------
	public void SpawnExplosionVFX(Vector3 position, Quaternion rotation)
	{
		ObjectPooler.Instance.SpawnFromPool(explosionVFXPool, position, rotation);
	}

	//public void SpawnFrostVFX(Vector3 position, Quaternion rotation)
	//{
		//ObjectPooler.Instance.SpawnFromPool(frostVFXPool, position, rotation);
	//}
}
