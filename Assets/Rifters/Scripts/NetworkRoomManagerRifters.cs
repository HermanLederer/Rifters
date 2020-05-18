using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Linq;

public class NetworkRoomManagerRifters : NetworkRoomManager
{
    [Header("Offline UI")]
    [SerializeField] private InputField ipAddressInputField = null;

    public List<NetworkRoomPlayerRifters> roomPlayers { get; } = new List<NetworkRoomPlayerRifters>();

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
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

    public void StartGame()
    {
        foreach (var player in roomPlayers)
        {
            player.ChangeUIToGame();
        }
        ServerChangeScene(GameplayScene);
    }
}
