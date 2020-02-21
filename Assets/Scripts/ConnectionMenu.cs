using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionMenu : MonoBehaviourPunCallbacks
{
	// Other components

	// Editor variables
	public GameObject mainPanel;
	public GameObject waitingPanel;
	public Text waitingText;

	// Public variables

	// Private variables

	//--------------------------
	// MonoBehaviour events
	//--------------------------
	void Awake()
	{
		//PhotonNetwork.AutomaticallySyncScene = true;
	}

	void Start()
	{
		
	}

	void Update()
	{

	}

	//--------------------------
	// MonoBehaviourPunCallbacks events
	//--------------------------
	public override void OnConnectedToMaster()
	{
		waitingText.text = "Connected";

		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log($"Disconnected due to {cause}");
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Joining random room failed:" + message);

		Debug.Log("Creating new room");
		PhotonNetwork.CreateRoom(Time.time + "", new RoomOptions { MaxPlayers = 10 });
	}

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
		{
			waitingText.text = "Waiting other players. Room ID: " + PhotonNetwork.CurrentRoom.Name;
		}
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
		{
			waitingText.text = "Starting Game";

			PhotonNetwork.LoadLevel("Game");
		}
	}

	//--------------------------
	// ConnectionMenu methods
	//--------------------------
	public void FindMatch()
	{
		PhotonNetwork.AutomaticallySyncScene = true;

		if (PhotonNetwork.IsConnected)
		{
			mainPanel.SetActive(false);
			waitingPanel.SetActive(true);
			waitingText.text = "Conecting...";

			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			PhotonNetwork.GameVersion = Application.version;
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
