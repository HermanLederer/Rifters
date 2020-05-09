using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	// Singleton
	public static AudioManager instance { get; private set; }
	// I consider not having an AudioManager initialized an error and don't think we should correct it automatically
	//public static AudioManager Instance
	//{
	//	get
	//	{
	//		if (instance == null)
	//		{
	//			instance = FindObjectOfType<AudioManager>();
	//			if (instance == null)
	//			{
	//				instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
	//			}
	//		}
	//		return instance;
	//	}
	//}

	//
	// Editor variables
	#region Editor variables
	[Header("Audio sources")]
	[SerializeField] private AudioSource musicSource = null;
	[SerializeField] private AudioSource drumSource = null; // TODO: use object pool instead
	[SerializeField] private AudioSource tribeSource = null; // TODO: use object pool instead
	[Header("Music")]
	[SerializeField] private AudioClip[] music = null;
	#endregion

	private bool firstMusicSourceIsPlaying;

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

		//sfxSource = this.gameObject.AddComponent<AudioSource>(); // and this is going to be offloaded off this component
	}

	private void Start()
	{
	}

	private void Update()
	{
		//check if music is playing and if it's not randomly start one of the tracks
		if (!musicSource.isPlaying)
		{
			PlayMusic(music[Random.Range(0,music.Length)]);
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
		drumSource.PlayOneShot(drumClip);
		// TODO: use PlayIn3D() instead
		// TODO: make it so there are multiple locations the drum can play at and randomly select one each time
	}

	public void PlayTribeVoc(AudioClip vocClip)
	{
		tribeSource.PlayOneShot(vocClip);
		// TODO: use PlayIn3D() instead
		// TODO: make it so there are multiple locations the vocal can play at and randomly select one each time
	}

	// Object pool
	// TODO: implement the object pool methods under here
	public void PlayIn2D(AudioClip clip, float volume)
	{
		// get an object that would play the clip without any spacial effects
	}

	public void PlayIn3D(AudioClip clip, float volume, Vector3 position, float minRadius, float maxRadius)
	{
		// get an object that would play the clip in 3d space
	}
}
