using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkJoinButton : MonoBehaviour {

    /// <summary>
    /// Button observed
    /// </summary>
    public Button joinRoom;

    // Use this for initialization
    void Start () {
        Button btn = joinRoom.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        PlayerSettings.PlayerRoomName = joinRoom.transform.GetChild(0).GetComponent<Text>().text;
        PlayerSettings.PlayGame = NetworkManager.GameType.PVE;
        PlayerSettings.JoiningRoom = true;
        SceneManager.LoadScene("LoadingSoloPlay");
        //NetworkManager.networkManager.joinRoom(joinRoom.transform.GetChild(0).GetComponent<Text>().text);
    }
}
