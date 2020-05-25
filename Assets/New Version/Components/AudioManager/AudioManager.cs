using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	// Singleton
	public static AudioManager instance { get; private set; }

	//
	// Editor variables
	#region Editor variables
	[Header("Audio sources")]
	[SerializeField] private AudioSource musicSource = null;
	[SerializeField] private List<Transform> drumLocations = null;
	[SerializeField] private List<Transform> tribeLocations = null;
	[Header("Audio parameter")]
	public float tribeVolume = 1;
	public float tribeMinDistance = 0;
	public float tribeMaxDistance = 0;
	public float drumVolume = 1;
	public float drumMinDistance = 0;
	public float drumMaxDistance = 0;
	[Header("Music")]
	[SerializeField] private AudioClip[] music = null;
	[Header("Pool prefabs")]
	[SerializeField] private GameObject soundObject2dPrefab = null;
	[SerializeField] private GameObject soundObject3dPrefab = null;
	#endregion

	private bool firstMusicSourceIsPlaying;
	private const string pool2d = "AudioPool2d";
	private const string pool3d = "AudioPool3d";

	//--------------------------
	// MonoBehaviour methods
	//--------------------------
	private void Awake()
	{
		// singleton
		if (instance != null && instance != this)
		{
			Debug.LogError("Impossible to initiate more than one AudioManager. Destroying the instance...");
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
		ObjectPooler.Instance.CreateNewPool(pool2d, soundObject2dPrefab, 5);
		ObjectPooler.Instance.CreateNewPool(pool3d, soundObject3dPrefab, 60);
	}

	private void Update()
	{
		//check if music is playing and if it's not randomly start one of the tracks
		if (!musicSource.isPlaying)
		{
			PlayMusic(music[Random.Range(0, music.Length)]);
		}
	}

	//--------------------------
	// AudioManager methods
	//--------------------------
	// Audio sources
	public void PlayMusic(AudioClip musicClip)
	{
		musicSource.clip = musicClip;
		musicSource.Play();
	}

	public void StopMusic(AudioClip musicClip)
	{
		musicSource.Stop();
	}

	public void PlayDrum(AudioClip drumClip)
	{
		PlayIn3D(drumClip, drumVolume, drumLocations[Random.Range(0, drumLocations.Count)].position, drumMinDistance, drumMaxDistance);
	}

	public void PlayTribeVoc(AudioClip vocClip)
	{
		PlayIn3D(vocClip, tribeVolume, tribeLocations[Random.Range(0, tribeLocations.Count)].position, tribeMinDistance, tribeMaxDistance);
	}

	public void PlayIn2D(AudioClip clip, float volume)
	{
		GameObject obj = ObjectPooler.Instance.SpawnFromPool(pool2d, Vector3.zero, Quaternion.identity);
		AudioSource source = obj.GetComponent<AudioSource>();
		source.volume = volume;
		source.clip = clip;
		source.Play();
	}

	public void PlayIn3D(AudioClip clip, float volume, Vector3 position, float minDistance, float maxVolume)
	{
		GameObject obj = ObjectPooler.Instance.SpawnFromPool(pool3d, position, Quaternion.identity);
		AudioSource source = obj.GetComponent<AudioSource>();
		source.volume = volume;
		source.minDistance = minDistance;
		source.maxDistance = maxVolume;
		source.clip = clip;
		source.Play();
	}
}
