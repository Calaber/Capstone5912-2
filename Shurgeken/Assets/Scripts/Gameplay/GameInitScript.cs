using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameInitScript : MonoBehaviour
{

    public static GameInitScript gis;

    [SerializeField]
    Text connectionText;

    [SerializeField]
    Transform[] spawnPoints;

    [SerializeField]
    Transform[] jailSpawnPoints;

    [SerializeField]
    Transform[] aiSpawnPoints;

    [SerializeField]
    Transform[] aiWayPoints;

    [SerializeField]
    Transform[] flagSpawns;

    [SerializeField]
    Camera sceneCamera;

    [SerializeField]
    AudioListener sceneListener;

    [SerializeField]
    int enemyAiCount;

    GameObject player;

    public GameObject redFlag;

    private NetworkManager networkManager;

    public PhotonView playerTracker;

    public PhotonView gameMaster;

    // Use this for initialization
    void Start()
    {
        gis = this;
        networkManager = NetworkManager.networkManager;
    }



    void Update()
    {
        if (connectionText)
            connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();

        if (redFlag == null)
        {
            redFlag = GameObject.FindGameObjectWithTag("RedFlag");
        }
        if (playerTracker == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Tracker");
            if (go != null)
                playerTracker = go.GetComponent<PhotonView>();
        }
        if (gameMaster == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("gm");
            if (go != null)
            {
                gameMaster = go.GetComponent<PhotonView>();
                Debug.Log("Assigned game master.");
            }
        }
    }

    void OnJoinedRoom()
    {
        StartSpawnProcess(0);
    }

    public IEnumerator SpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, spawnPoints.Length);
        player = networkManager.spawnObject("Player", spawnPoints[index], null);

        player.GetComponent<PlayerNetworkController>().RespawnMe += StartSpawnProcess;
        sceneCamera.enabled = false;
        sceneListener.enabled = false;
        // --target--.GetComponent<PhotonView>().RPC("asdf", PhotonTargets.All,params);
    }

    public IEnumerator SpawnPlayerInJail(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, jailSpawnPoints.Length);
        player = networkManager.spawnObject("Player", jailSpawnPoints[index], null);

        // Mark that player is in jail
        playerTracker.RPC("AddPlayerToJail", PhotonTargets.All, player.GetComponent<PhotonView>().ownerId, player.GetComponent<DataController>().team);

        //Start spawn process and switch to the new camera and listener
        player.GetComponent<PlayerNetworkController>().RespawnMe += StartSpawnProcess;
        sceneCamera.enabled = false;
        sceneListener.enabled = false;
    }

    public IEnumerator SpawnAI()
    {

        GameObject guard;
        int index = Random.Range(0, aiSpawnPoints.Length);
        guard = networkManager.spawnSceneObject("Guard", aiSpawnPoints[index], null);
        if (guard)
            guard.GetComponent<EnemyStatePattern>().setPatrolPath(new TestPath(aiWayPoints));
        yield return null;
    }

    public IEnumerator SpawnFlag()
    {
        int index = Random.Range(0, flagSpawns.Length);
        redFlag = networkManager.spawnSceneObject("Red Flag 1", flagSpawns[index], null);
        redFlag.GetComponent<FlagController>().spawnPoint = flagSpawns[index];
        redFlag.transform.gameObject.layer = 11;
        yield return null;
    }

    public IEnumerator SpawnPlayerTracker()
    {
        playerTracker = networkManager.spawnSceneObject("PlayerTracker", null).GetComponent<PhotonView>();
        yield return null;
    }

    public IEnumerator SpawnGameMaster()
    {
        gameMaster = networkManager.spawnSceneObject("GameMaster", null).GetComponent<PhotonView>();
        yield return null;
    }

    public void StartSpawnProcess(float respawnTime)
    {
        sceneCamera.enabled = true;
        StartCoroutine("SpawnPlayer", respawnTime);
        if (NetworkManager.networkManager.isMaster())
        {
            StartCoroutine("SpawnPlayerTracker");
            StartCoroutine("SpawnGameMaster");
            StartCoroutine("SpawnFlag");
            for (int i = 0; i < enemyAiCount; i++)
            {
                StartCoroutine("SpawnAI");
            }
        }
    }
}
