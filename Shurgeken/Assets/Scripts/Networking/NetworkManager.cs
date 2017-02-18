using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    public enum GameType
    {
        SINGLE,
        PVE,
        PVP
    }

    [SerializeField]
    Text connectionText;

    [SerializeField]
    Transform[] spawnPoints;

    [SerializeField]
    Transform[] aiSpawnPoints;

    [SerializeField]
    Camera sceneCamera;

    [SerializeField]
    AudioListener sceneListener;

    GameObject player;

    /// <summary>
    /// Static version of this. Allows abstract calls from anywhere
    /// </summary>
    public static NetworkManager networkManager;

    void Awake()
    {
        if (networkManager == null)
        {
            DontDestroyOnLoad(gameObject);
            networkManager = this;
        }
        // Already assigned, destroy this one.
        else if (networkManager != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.2");
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void joinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void leaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    void Update()
    {
        connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    public RoomInfo[] getRoomList()
    {
        return PhotonNetwork.GetRoomList();
    }

    public void loadLevel(int levelNumber)
    {
        PhotonNetwork.LoadLevel(levelNumber);
    }

    public void loadLevel(string levelName)
    {
        PhotonNetwork.LoadLevel(levelName);
    }

    void OnJoinedLobby()
    {
        createRoom("TestLevel", GameType.PVP);
    }

    public void joinRoom(string gameName)
    {
        PhotonNetwork.JoinRoom(gameName);
    }

    /// <summary>
    /// This can only be called by the master or owner of the game object
    /// </summary>
    /// <param name="obj">object to destroy</param>
    public void Destroy(GameObject obj)
    {
        PhotonNetwork.Destroy(obj);
    }

    public void createRoom(string gameName, GameType type)
    {
        byte players = 4;
        switch(type)
        {
            case GameType.PVE:
                PhotonNetwork.offlineMode = false;
                players = 4;
                break;
            case GameType.PVP:
                PhotonNetwork.offlineMode = false;
                players = 8;
                break;
            case GameType.SINGLE:
                PhotonNetwork.offlineMode = true;
                players = 1;
                break;
            default:
                Debug.Log("Invalid GameType provided");
                break;
        }
        RoomOptions ro = new RoomOptions() { IsVisible = true, MaxPlayers = players };
        PhotonNetwork.JoinOrCreateRoom(gameName, ro, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        StartSpawnProcess(0f);
    }

    public void StartSpawnProcess(float respawnTime)
    {
        sceneCamera.enabled = true;
        StartCoroutine("SpawnPlayer", respawnTime);
    }

    public bool isMaster()
    {
        return PhotonNetwork.isMasterClient;
    }

    public IEnumerator SpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, spawnPoints.Length);
        player = PhotonNetwork.Instantiate("Player",
                                           spawnPoints[index].position,
                                           spawnPoints[index].rotation,
                                           0);
        player.GetComponent<PlayerNetworkController>().RespawnMe += StartSpawnProcess;
        sceneCamera.enabled = false;
        sceneListener.enabled = false;
    }

    public IEnumerator SpawnAI()
    {

        int index = Random.Range(0, spawnPoints.Length);
        player = PhotonNetwork.Instantiate("Guard",
                                           spawnPoints[index].position,
                                           spawnPoints[index].rotation,
                                           0);
        yield return null;
    }
}