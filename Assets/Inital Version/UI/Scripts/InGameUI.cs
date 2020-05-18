using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
	// Editor variables
	[Header("Panels")]
	public GameObject menuPanel;
	public GameObject returnToMenuPanel;
	public GameObject winningPanel;
	public GameObject losingPanel;
	[Header("Text fields")]
	public Text score1;
	public Text score2;
	public Text yourTeam1;
	public Text yourTeam2;

	// Public variables

	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (menuPanel.activeSelf)
			{
				menuPanel.SetActive(false);
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				menuPanel.SetActive(true);
				Cursor.lockState = CursorLockMode.None;
			}
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			LoadWinningScreen();
		}
	}

	//--------------------------
	// PauseMenu events
	//--------------------------
	public void ResumeGame()
	{
		menuPanel.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void GoToMainMenu()
	{
		returnToMenuPanel.SetActive(true);

		SceneManager.LoadScene("MainMenu");
	}

	public void ReloadGame()
	{
		// TODO: sswitch to photon scene loading if multiplayer is necessary
		//PhotonNetwork.LoadLevel("Game");
		SceneManager.LoadScene("Runner prototype");
	}

	public void LoadWinningScreen()
	{
		winningPanel.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
	}

	public void LoadLosingScreen()
	{
		losingPanel.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
	}
}
