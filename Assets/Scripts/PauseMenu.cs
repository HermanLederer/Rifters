using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PauseMenu : MonoBehaviour
{
	// Editor variables
	public GameObject menuPanel;
	public GameObject returnToMenuPanel;
	public GameObject winningPanel;
	public GameObject losingPanel;

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

		PhotonNetwork.Disconnect();
		PhotonNetwork.LoadLevel("Menu");
	}

	public void ReloadGame()
	{
		PhotonNetwork.LoadLevel("Game");
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
