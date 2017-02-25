using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    public enum GameType
    {
        SINGLE,
        PVE,
        PVP
    }

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
            GameObject.Destroy(this.gameObject);
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

    public bool isInLobby()
    {
        return PhotonNetwork.insideLobby;
    }

    public void leaveLobby()
    {
        if (isInLobby())
            PhotonNetwork.LeaveLobby();
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

    public void joinRoom(string gameName)
    {
        PhotonNetwork.JoinRoom(gameName);
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
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

    public GameObject spawnSceneObject(string objectName, Transform trans, Object[] data)
    {
        return spawnSceneObject(objectName, trans.position, trans.rotation, data);
    }

    public GameObject spawnSceneObject(string objectName, Vector3 pos, Quaternion rot, Object[] data)
    {
        return spawnSceneObject(objectName, pos, rot, 0, data);
    }

    public GameObject spawnSceneObject(string objectName, Vector3 pos, Quaternion rot, int group, Object[] data)
    {
        if (isMaster()) {
            return PhotonNetwork.InstantiateSceneObject(objectName,
                                                  pos,
                                                  rot,
                                                  group,
                                                  data);
        }
        else
        {
            return null;
        }
    }

    public GameObject spawnObject(string objectName, Transform trans, Object[] data)
    {
        return spawnObject(objectName, trans.position, trans.rotation, data);
    }

    public GameObject spawnObject(string objectName, Vector3 pos, Quaternion rot, Object[] data)
    {
        return spawnObject(objectName, pos, rot, 0, data);
    }

    public GameObject spawnObject(string objectName, Vector3 pos, Quaternion rot, int group, Object[] data)
    {
        return PhotonNetwork.Instantiate(objectName,
                                              pos,
                                              rot,
                                              group,
                                              data);
    }

    public bool isMaster()
    {
        return PhotonNetwork.isMasterClient;
    }
}