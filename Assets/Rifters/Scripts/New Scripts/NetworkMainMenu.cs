using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerRifter networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPanel = null;

    public void HostLobby()
    {
        networkManager.StartHost();

        landingPanel.SetActive(false);
    }
}
