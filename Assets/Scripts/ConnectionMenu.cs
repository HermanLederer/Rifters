using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionMenu : MonoBehaviourPunCallbacks
{
	// Other components
	private CharacterController _characterController;

	// Editor variables
	public PlayerMovement playerMovement;

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
		//Screen.fullScreen = false;
		//PhotonNetwork.AutomaticallySyncScene = true;
	}

	void Start()
	{
		//FindMatch();
	}

	void Update()
	{

	}

	//--------------------------
	// MonoBehaviourPunCallbacks events
	//--------------------------
	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to master");

		mainPanel.SetActive(false);
		waitingPanel.SetActive(true);
		waitingText.text = "Conecting...";

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
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 10 });
	}

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
		{
			waitingText.text = "Waiting other players";

			Debug.Log("Joined an empty room, waiting for it to fill");
		}
	}

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
		{
			Debug.Log("Room filled, starting the game");

			waitingText.text = "Starting Game";

			PhotonNetwork.LoadLevel("Game");
		}
	}

	// doesnt work
	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		Debug.Log("Player left the room, going back to menu");
		PhotonNetwork.LoadLevel("Menu");
		PhotonNetwork.LeaveRoom();
	}

	//--------------------------
	// ConnectionMenu events
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
}
