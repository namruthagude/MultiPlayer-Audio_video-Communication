using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Text RoomName;
    public GameObject spawnParent;
    public BoxCollider2D spawnArea;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }
    
    public void CreateRoom()
    {
        string roomName = createInput.text;
        RoomName.text = roomName;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 10;
        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");

    }
    public void JoinRoom()
    {
        string roomName = joinInput.text;
        RoomName.text = roomName;
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Vector3 spawnPos = GetRandomPos();
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
    }
    Vector3 GetRandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), 0);
        return randomPos;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
       Debug.LogError("Can't create room because " +message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Can't join room because " + message);
    }
}
