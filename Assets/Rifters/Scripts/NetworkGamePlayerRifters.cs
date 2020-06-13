using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public enum TypeOfSpell
{
    FIREBALL,
    STOPSPELL,
    BLINK
}

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

    [SerializeField] private Image fireSpellImage = null;
    [SerializeField] private Image stopSpellImage = null;
    [SerializeField] private Image blinkSpellImage = null;

    [Header("Pause UI")]
    [SerializeField] private GameObject PauseUI = null;

    [Header("Pause UI Components")]
    [SerializeField] private Text pauseTimeText = null;
    [SerializeField] private Text pauseScore = null;

    [SyncVar]
    private string displayName = "PlaceHolder...";

    public bool isPaused = false;

    [HideInInspector]
    public Player myPlayer = null;

    private NetworkManagerRifter room;

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
                ResumeGame();
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
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        GameUI.SetActive(true);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    public void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.GamePlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
        }

        myScore.text = isClientOnly ? Room.team2score.ToString() : Room.team1score.ToString();
        opponentScore.text = isClientOnly ? Room.team1score.ToString() : Room.team2score.ToString();

        pauseScore.text = myScore.text + " : " + opponentScore.text;
    }

    [ClientRpc]
    public void RpcUpdateTimer(float _gameTime)
    {
        //Debug.Log("Tiempo de juego: " + _gameTime + " del jugador: " + netId);

        string minutes = Mathf.Floor(_gameTime / 60).ToString();
        string seconds = Mathf.RoundToInt(_gameTime % 60).ToString("00");

        gameTimeText.text = minutes + ":" + seconds;
        pauseTimeText.text = minutes + ":" + seconds;
    }

    public void ResumeGame()
    {
        PauseUI.SetActive(false);
        GameUI.SetActive(true);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ChangeSpellAlpha(TypeOfSpell spell, float alphaValue)
    {
        Color tempColor = Color.white;
        switch (spell)
        {
            case TypeOfSpell.FIREBALL:
                tempColor = fireSpellImage.color;
                tempColor.a = alphaValue;
                fireSpellImage.color = tempColor;
                break;
            case TypeOfSpell.STOPSPELL:
                tempColor = stopSpellImage.color;
                tempColor.a = alphaValue;
                stopSpellImage.color = tempColor;
                break;
            case TypeOfSpell.BLINK:
                tempColor = blinkSpellImage.color;
                tempColor.a = alphaValue;
                blinkSpellImage.color = tempColor;
                break;
        }
    }

    [ClientRpc]
    public void RpcResetPlayerPosition()
    {
        myPlayer.ResetPlayerPosition();
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }


}
