using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        NetworkManager.networkManager.joinRoom(joinRoom.transform.GetChild(0).GetComponent<Text>().text);


    }
}
