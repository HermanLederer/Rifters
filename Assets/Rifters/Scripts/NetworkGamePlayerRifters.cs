using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SyncVar]
    private string displayName = "PlaceHolder...";

    private NetworkManagerRifter room;

    private NetworkManagerRifter Room
    {
        get
        {
            if (room != null) { return room; }

            return room = NetworkManager.singleton as NetworkManagerRifter;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
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
