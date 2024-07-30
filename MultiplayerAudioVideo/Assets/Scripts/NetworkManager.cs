using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Fusion;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using System;
using static Unity.Collections.Unicode;

public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkManager Instance;
    private NetworkRunner _networkRunner;
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Text RoomName;
    public NetworkPrefabRef PlayerPrefab;
    //public GameObject spawnParent;
    public BoxCollider2D spawnArea;

    void Awake()
    {
        _networkRunner = GetComponent<NetworkRunner>();
        _networkRunner.ProvideInput = true;
        _networkRunner.AddCallbacks(this);
       
    }
    //// Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        
    }
    public void CreateRoom()
    {
        RoomName.text = createInput.text;
        StartGame(GameMode.Host);
    }
    public void JoinRoom()
    {
        RoomName.text = joinInput.text;
        StartGame(GameMode.Client);
    }
    public void StartGame(GameMode mode)
    {
        _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = RoomName.text, // Name of the room
            //Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
       
        Debug.Log($"Player {player.PlayerId} joined");
        Vector3 spawnPos = GetRandomPos();
        var playerObject = runner.Spawn(PlayerPrefab, spawnPos, Quaternion.identity, player);
        if(playerObject == null)
        {
            Debug.Log("Failed to spawn player");
        }
    }
     Vector3 GetRandomPos()
    {
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), 0);
        return spawnPos;
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player {player.PlayerId} left");
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) {
        NetworkInputData data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            data.movementInput.y = 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            data.movementInput.x = -1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            data.movementInput.y = -1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            data.movementInput.x = 1;

        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("Object exited AOI");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("Object entered AOI");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }
}
