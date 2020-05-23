using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

public class NetworkRoomPlayerRifters : NetworkBehaviour//NetworkRoomPlayer
{
    #region OldScript
    /*
    [Header("Room UI")]
    [SerializeField] private GameObject RoomUI = null;
    [SerializeField] private GameObject[] PlayerCards = null;
    [SerializeField] private Image[] PlayerAvatars = null;
    [SerializeField] private Text[] PlayerNames = null;
    [SerializeField] private Text[] PlayerStates = null;

    [SerializeField] private Button startButton = null;

    private bool showStartButton;

    [Header("Game UI")]
    [SerializeField] private GameObject GameUI = null;
    

    [Header("Player Settings")]
    [SerializeField] private Sprite m_avatar;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "PlaceHolder...";

    [SyncVar(hook = nameof(HandleAvatarColorChanged))]
    private Color AvatarColor = Color.white;

    private NetworkRoomManagerRifters room;

    public NetworkRoomManagerRifters Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkRoomManagerRifters;
        }
    }

    //-- Enables the UI Player GameObject we have authority on --
    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerPrefs.GetString("PlayerName"));
        CmdSetAvatarColor(PlayerNameAndAvatar.AvatarColors[PlayerPrefs.GetInt("PlayerAvatar")]);

        if (hasAuthority)
        {
            RoomUI.SetActive(true);
            if (showStartButton)
            {
                startButton.gameObject.SetActive(true);
            }
        }
    }

    //-- Update the UI when the client is connected to the server --
    public override void OnStartClient()
    {
        Room.roomPlayers.Add(this);

        UpdateRoomDisplay();
    }

    //-- Update the UI when the client is disconnected from the server --
    public override void OnStopClient()
    {
        Room.roomPlayers.Remove(this);

        UpdateRoomDisplay();
    }

    //-- Make the start button interactible or not for the host --
    //-- Update the UI when the client ready state changes --
    public override void OnClientReady(bool readyState)
    {
        if(index == 0)
        {
            if (!readyState)
            {
                startButton.interactable = false;
            }
        }

        UpdateRoomDisplay();
    }

    //-- Update the UI when the client name changes --
    public void HandleDisplayNameChanged(string oldValue, string newValue)
    {
        UpdateRoomDisplay();
    }

    //-- Update the UI when the client avatar changes --
    public void HandleAvatarColorChanged(Color oldvalue, Color newValue)
    {
        UpdateRoomDisplay();
    }

    //-- Update UI --
    public void UpdateRoomDisplay()
    {
        if (!hasAuthority)
        {
            foreach(var player in Room.roomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateRoomDisplay();
                    break;
                }
            }
            return;
        }

        //-- Reset UI --
        for (int i = 0; i < PlayerCards.Length; i++)
        {
            PlayerCards[i].SetActive(false);
        }

        //-- Modify the UI to match the current state --
        for (int i = 0; i < Room.roomPlayers.Count; i++)
        { 
            PlayerCards[i].SetActive(true);
            PlayerNames[i].text = Room.roomPlayers[i].DisplayName;
            PlayerAvatars[i].sprite = Room.roomPlayers[i].m_avatar;
            PlayerAvatars[i].color = Room.roomPlayers[i].AvatarColor;
            PlayerStates[i].text = Room.roomPlayers[i].readyToBegin? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
        }
    }

    public void UpdateGameDisplay()
    {

    }

    //-- Is called when all players are ready to start the game and enables the start button for the host --
    public void AllPlayersReady()
    {
        if(index == 0)
        {
            startButton.interactable = true;
        }
    }

    public void ReadyUp()
    {
        CmdChangeReadyState(!readyToBegin);
    }

    public void ChangeUIToGame()
    {
        RoomUI.SetActive(false);
        GameUI.SetActive(true);

        UpdateGameDisplay();
    }

    #region Getters & Setters

    public void SetShowStartButton(bool value)
    {
        showStartButton = value;
    }
    #endregion

    #region Commands
    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    private void CmdSetAvatarColor(Color color)
    {
        AvatarColor = color;
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.roomPlayers[0].connectionToClient != connectionToClient) { return; } //make sure the client doing this is the host

        Room.StartGame();
    }
    #endregion
    */
    #endregion

    [Header("UI")]
    [SerializeField] private GameObject roomUI = null;
    [SerializeField] private GameObject[] playerCards = new GameObject[4];
    [SerializeField] private Image[] playerAvatars = new Image[4];
    [SerializeField] private Text[] playerNameTexts = new Text[4];
    [SerializeField] private Text[] playerReadyTexts = new Text[4];
    [SerializeField] private Button startGameButton = null;

    [Header("Player Settings")]
    [SerializeField] private Sprite m_avatar = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "PlaceHolder...";
    [SyncVar(hook = nameof(HandleAvatarColorChanged))]
    private Color AvatarColor = Color.white;
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;

    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerRifter room;

    private NetworkManagerRifter Room
    {
        get
        {
            if(room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerRifter;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameAndAvatar.DisplayName);

        roomUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    public void HandleAvatarColorChanged(Color oldValue, Color newValue) => UpdateDisplay();
    
    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
        }

        for (int i = 0; i < playerCards.Length; i++)
        {
            playerCards[i].SetActive(false);
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            playerCards[i].SetActive(true);
            playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            playerAvatars[i].sprite = Room.RoomPlayers[i].m_avatar;
            playerAvatars[i].color = Room.RoomPlayers[i].AvatarColor;
            playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if(!isLeader) { return; }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if(Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();
    }
}
