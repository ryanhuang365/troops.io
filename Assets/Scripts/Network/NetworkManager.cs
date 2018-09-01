using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {


    public GameObject player;
    private const string roomName = "RoomName";
    private RoomInfo[] roomsList;
    public Transform spawnPoint;
    const string VERSION = "0.0.1";

    private void Start()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }
    void OnGUI()
    {
        if(!PhotonNetwork.connected)
        PhotonNetwork.ConnectUsingSettings(VERSION);
        if (!PhotonNetwork.connected)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null)
        {
            // Create Room
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                PhotonNetwork.CreateRoom(roomName + Guid.NewGuid().ToString("N"));

            // Join Room
            if (roomsList != null)
            {
                for (int i = 0; i < roomsList.Length; i++)
            {
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].Name))
                        PhotonNetwork.JoinRoom(roomsList[i].Name);
                }
            }
        }
    }

    void OnReceivedRoomListUpdate()
    {
        roomsList = PhotonNetwork.GetRoomList();
    }
    void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation, 0);
    }














    /*const string VERSION = "v0.0.1";
public string roomName = "VVR";
public string playerPrefabName = "player";
public Transform spawnPoint;

// Use this for initialization
void Start()
{
    PhotonNetwork.ConnectUsingSettings(VERSION);
}

void OnJoinedLobby()
{
    RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 4 };
    PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
}
void OnJoinedRoom()
{
    PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation, 0);
}*/
}

