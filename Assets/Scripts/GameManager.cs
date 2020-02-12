using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject playerPrefab;

    void Start()
    {
		PhotonNetwork.Instantiate(playerPrefab.name, transform.position + Vector3.one * Random.Range(-3, 3), transform.rotation);
    }
}
