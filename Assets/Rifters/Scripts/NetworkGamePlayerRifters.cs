using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayerRifters : NetworkBehaviour
{
    #region Old Script
    /*
    [SyncVar]
    private string displayName = "Loading...";

    private NetworkRoomManagerRifters room;

    public NetworkRoomManagerRifters Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkRoomManagerRifters;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);
        Room.gamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.gamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string _displayName)
    {
        displayName = _displayName;
    }
    */
    #endregion

    [Header("Game UI")]
    [SerializeField] private GameObject GameUI = null;

    [Header("Game UI Components")]
    [SerializeField] private Text myScore = null;
    [SerializeField] private Text opponentScore = null;

    [SerializeField] private Text gameTimeText = null;

    [SerializeField] private Image FireSpell = null;
    [SerializeField] private Image StopSpell = null;

    [Header("Pause UI")]
    [SerializeField] private GameObject PauseUI = null;

    [Header("Pause UI Components")]
    [SerializeField] private Text pauseTimeText = null;
    [SerializeField] private Text pauseScore = null;

    [SyncVar]
    private string displayName = "PlaceHolder...";

    private NetworkManagerRifter room;
    private bool isPaused = false;

    private NetworkManagerRifter Room
    {
        get
        {
            if (room != null) { return room; }

            return room = NetworkManager.singleton as NetworkManagerRifter;
        }
    }

    private void Update()
    {
        if (!hasAuthority)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                GameUI.SetActive(true);
                PauseUI.SetActive(false);
                isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                GameUI.SetActive(false);
                PauseUI.SetActive(true);
                isPaused = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
        GameUI.SetActive(true);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }
}
