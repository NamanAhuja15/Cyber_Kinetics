using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int playerIndex=0;
    [SerializeField]
    private Text connectionText;
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    public GameObject[] playerModel;
    [SerializeField]
    private GameObject serverWindow;
    [SerializeField]
    private GameObject messageWindow;
    [SerializeField]
    private GameObject SelectionWindow;
    [SerializeField]
    private InputField username;
    [SerializeField]
    private InputField roomName;
    [SerializeField]
    private InputField roomList;
    [SerializeField]
    private InputField messagesLog;
    [SerializeField]
    private Camera[] RenderCameras;
    [SerializeField]
    private GameObject crosshair;
    private GameObject player;
    private Queue<string> messages;
    private const int messageCount = 10;
    private string nickNamePrefKey = "PlayerName";

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        messages = new Queue<string>(messageCount);
        if (PlayerPrefs.HasKey(nickNamePrefKey))
        {
            username.text = PlayerPrefs.GetString(nickNamePrefKey);
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        connectionText.text = "Connecting to lobby...";
    }

    /// <summary>
    /// Called on the client when you have successfully connected to a master server.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// Called on the client when the connection was lost or you disconnected from the server.
    /// </summary>
    /// <param name="cause">DisconnectCause data associated with this disconnect.</param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        if(connectionText!=null)
        connectionText.text = cause.ToString();
    }

    /// <summary>
    /// Callback function on joined lobby.
    /// </summary>
    public override void OnJoinedLobby()
    {
        serverWindow.SetActive(true);
        foreach(Camera cam in RenderCameras)
        {
            cam.enabled = true;
        }
        connectionText.text = "";
    }

    /// <summary>
    /// Callback function on reveived room list update.
    /// </summary>
    /// <param name="rooms">List of RoomInfo.</param>
    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        roomList.text = "";
        foreach (RoomInfo room in rooms)
        {
            roomList.text += room.Name + "\n";
        }
    }

    /// <summary>
    /// The button click callback function for join room.
    /// </summary>
    public void JoinRoom()
    {
        serverWindow.SetActive(false);
        connectionText.text = "Joining room...";
        PhotonNetwork.LocalPlayer.NickName = username.text;
        PlayerPrefs.SetString(nickNamePrefKey, username.text);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            MaxPlayers = 4
        };
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
        }
        else
        {
            connectionText.text = "PhotonNetwork connection is not ready, try restart it.";
        }
    }

    /// <summary>
    /// Callback function on joined room.
    /// </summary>
    public override void OnJoinedRoom()
    {
        connectionText.text = "";
        
        Respawn(0f);
    }

    /// <summary>
    /// Start spawn or respawn a player.
    /// </summary>
    /// <param name="spawnTime">Time waited before spawn a player.</param>
   public void Respawn(float spawnTime)
    {
        StartCoroutine(RespawnCoroutine(spawnTime));
        foreach (Camera cam in RenderCameras)
        {
            cam.enabled = false;
        }

    }

    /// <summary>
    /// The coroutine function to spawn player.
    /// </summary>
    /// <param name="spawnTime">Time waited before spawn a player.</param>
    IEnumerator RespawnCoroutine(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        messageWindow.SetActive(true);
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        player = PhotonNetwork.Instantiate(playerModel[playerIndex].name, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation, 0);
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.RespawnEvent += Respawn;
        playerHealth.AddMessageEvent += AddMessage;
        playerHealth.crosshair = crosshair.GetComponent<Image>();
        sceneCamera.enabled = false;
        crosshair.SetActive(true);
        if (spawnTime == 0)
        {
            AddMessage("Player " + PhotonNetwork.LocalPlayer.NickName + "has entered the arena");
        }
        else
        {
            AddMessage("Player " + PhotonNetwork.LocalPlayer.NickName + "Respawned");
        }
    }

    /// <summary>
    /// Add message to message panel.
    /// </summary>
    /// <param name="message">The message that we want to add.</param>
    void AddMessage(string message)
    {
        photonView.RPC("AddMessage_RPC", RpcTarget.All, message);
    }

    /// <summary>
    /// RPC function to call add message for each client.
    /// </summary>
    /// <param name="message">The message that we want to add.</param>
    [PunRPC]
    void AddMessage_RPC(string message)
    {
        messages.Enqueue(message);
        if (messages.Count > messageCount)
        {
            messages.Dequeue();
        }
        messagesLog.text = "";
        foreach (string m in messages)
        {
            messagesLog.text += m + "\n";
        }
    }

    /// <summary>
    /// Callback function when other player disconnected.
    /// </summary>
    public override void OnPlayerLeftRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            AddMessage("Player " + other.NickName + " is a coward,he left the game.");
        }
    }
    public void SelectPlayer1()
    {
        playerIndex = 0;
        RenderCameras[playerIndex].GetComponent<Lobby>().Animate();
        SelectionWindow.SetActive(false);
    }   
    public void SelectPlayer2()
    {
        playerIndex = 1;
        RenderCameras[playerIndex].GetComponent<Lobby>().Animate();
        SelectionWindow.SetActive(false);
    }
    public void SelectPlayer3()
    {
        playerIndex = 2;
        RenderCameras[playerIndex].GetComponent<Lobby>().Animate();
        SelectionWindow.SetActive(false);
    }
}
