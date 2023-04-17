using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    public Button btnMultiplayer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Conectando con el servidor...");
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Uniendo a un lobby.");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Preparado para jugar multijugador.");
        btnMultiplayer.interactable = true;
    }

    public void FindMatch()
    {
        Debug.Log("Buscando sala...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        MakeRoom();
    }

    private void MakeRoom()
    {
        int randomRoomNum = Random.Range(0, 5000);

        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 6,
            PublishUserId = true
        };

        PhotonNetwork.CreateRoom($"RoomName_{randomRoomNum}",roomOptions);
        Debug.Log($"Sala creada : {randomRoomNum}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Cargar escena del juego Multiplayer");
        PhotonNetwork.LoadLevel(2);
    }
}
