using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

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

    void Start()
    {

        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.2");

    }

    void Update()
    {
        connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    void OnJoinedLobby()
    {
        RoomOptions ro = new RoomOptions() { IsVisible = true, MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("TestRoom", ro, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        StartSpawnProcess(0f);
    }

    void StartSpawnProcess(float respawnTime)
    {
        sceneCamera.enabled = true;
        StartCoroutine("SpawnPlayer", respawnTime);
    }

    IEnumerator SpawnPlayer(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        int index = Random.Range(0, spawnPoints.Length);
        player = PhotonNetwork.Instantiate("FirstPersonNinja",
                                           spawnPoints[index].position,
                                           spawnPoints[index].rotation,
                                           0);
        player.GetComponent<NetworkController>().RespawnMe += StartSpawnProcess;
        sceneCamera.enabled = false;
        sceneListener.enabled = false;
    }
}