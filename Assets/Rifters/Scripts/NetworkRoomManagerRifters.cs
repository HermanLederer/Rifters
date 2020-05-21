using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;
using System;

public class NetworkRoomManagerRifters : NetworkRoomManager
{
    #region Old Network Manager
    /*
    [Header("Offline UI")]
    [SerializeField] private InputField ipAddressInputField = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerRifters gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;

    public List<NetworkRoomPlayerRifters> roomPlayers { get; } = new List<NetworkRoomPlayerRifters>();

    public List<NetworkGamePlayerRifters> gamePlayers { get; } = new List<NetworkGamePlayerRifters>();

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if(!IsSceneActive(RoomScene))
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("Soy: " + conn.identity.gameObject);
        if(conn != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerRifters>();

            roomPlayers.Remove(player);
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnRoomServerPlayersReady()
    {
        foreach (var player in roomPlayers)
        {
            player.AllPlayersReady();
        }
    }

    public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnection conn)
    {
        GameObject newPlayer = Instantiate(roomPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);

        NetworkRoomPlayerRifters roomPlayer = newPlayer.GetComponent<NetworkRoomPlayerRifters>();

        if(roomPlayer.index == 0)
        {
            roomPlayer.SetShowStartButton(true);
        }

        return newPlayer;
    }

    public void JoinLobby()
    {
        string ipAddress = ipAddressInputField.text;

        networkAddress = ipAddress;
        StartClient();
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }

    public void StartGame()
    {
        foreach (var player in roomPlayers)
        {
            player.ChangeUIToGame();
        }
        ServerChangeScene(GameplayScene);
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (IsSceneActive(RoomScene) && newSceneName.Contains("Game"))
        {
            for (int i = roomPlayers.Count - 1; i >= 0; i--)
            //for (int i = 0; i < roomPlayers.Count; i++)
            {
                var conn = roomPlayers[i].connectionToClient;
                var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.SetDisplayName(roomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject, true);
            }
        }
        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.Contains("Game"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }
    */
    #endregion
}
