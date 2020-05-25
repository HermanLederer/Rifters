using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;

public class NetworkManagerRifter : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;
    [Scene] [SerializeField] private string gameScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerRifters roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerRifters gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;
    [SerializeField] private GameObject ballPrefab = null;
    private GameObject ballInstance = null;

    [Header("Game Conditions")]
    [SerializeField] private int playToGoals = 5;
    [HideInInspector] public int team1score = 0;
    [HideInInspector] public int team2score = 0;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;

    public List<NetworkRoomPlayerRifters> RoomPlayers { get; } = new List<NetworkRoomPlayerRifters>();
    public List<NetworkGamePlayerRifters> GamePlayers { get; } = new List<NetworkGamePlayerRifters>();

    public void Update()
    {
        if (IsSceneActive(gameScene))
        {
            if (team1score >= playToGoals || team2score >= playToGoals)
            {
                if (team1score > team2score)
                    DeclareWiner(GameTeam.Team1);
                else
                    DeclareWiner(GameTeam.Team2);
            }

            if (ballInstance != null)
            {
                Debug.Log("Ball position: " + ballInstance.transform.position);
            }
        }
    }

        
		
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
        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (!IsSceneActive(menuScene))
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (IsSceneActive(menuScene))
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerRifters roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }

        if (IsSceneActive(gameScene))
        {

        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerRifters>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();

        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if(numPlayers < minPlayers) { return false; }

        foreach (var player in RoomPlayers)
        {
            if(!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if (IsSceneActive(menuScene))
        {
            if(!IsReadyToStart()) { return; }

            ServerChangeScene(gameScene);
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if(IsSceneActive(menuScene) && newSceneName.Contains("Game"))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject, true);
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

            ballInstance = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.Spawn(ballInstance);
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }

    public void DeclareWiner(GameTeam team)
    {
        Debug.Log(team + " won!");
        //isGamePlaying = false;
        Time.timeScale = 0.1f;
    }

    public void respawnPlayersAndBall()
    {
        //AudioManager.instance.PlayJump(Goal);
        ballInstance.transform.position = Vector3.zero;
        ballInstance.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }

    public void Score(GameTeam team)
    {
        // increasing the score
        if (team == GameTeam.Team1) ++team1score;
        else ++team2score;

        //inGameUI.score1.text = "" + team1score;
        //inGameUI.score2.text = "" + team2score;

        //Debug.Log("A goal has been scored by " + team);
        //Debug.Log("" + GameTeam.Team1 + ": " + team1score + "; " + GameTeam.Team2 + ": " + team2score + ";");

        //AudioManager.instance.PlayDrum(goalDrums);
        //AudioManager.instance.PlayTribeVoc(goalVoc);

        respawnPlayersAndBall();
    }
}
