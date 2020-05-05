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
	[Header("Audio clips")]
	[SerializeField] private AudioClip[] music = null;
	[SerializeField] private AudioClip Goal = null;
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

		// I find this a bad idea because we can't change any parameters this way, instead we're gonna have the sources in the prefab;
		// Create audio sources, and save them as references
		//musicSource = this.gameObject.AddComponent<AudioSource>();
		//musicSource2 = this.gameObject.AddComponent<AudioSource>(); // also we don't need another source
		//sfxSource = this.gameObject.AddComponent<AudioSource>(); // and this is going to be offloaded off this component
	}

	private void Start()
	{
		PlayMusic(music[0]);
	}

	//--------------------------
	// AudioManager methods
	//--------------------------
	public void PlayMusic(AudioClip musicClip)
	{
		musicSource.clip = musicClip;
		musicSource.Play();
	}

	public void StopMusic(AudioClip musicClip)
	{
		musicSource.Stop();
	}

	// Omitted some methods
}
