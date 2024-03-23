using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject player;
    [Space]
    public Transform spawnPoint;
    [Space] 
    public GameObject roomCam;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("connecting...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("connected to sever");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("test", null, null);

        Debug.Log("We're in the lobby");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("We're connected and in a room!");

        roomCam.SetActive(false);

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;
    }

}
