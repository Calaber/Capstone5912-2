using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateMultiplayerRoomButton : MonoBehaviour {

    public InputField roomName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreatePvPRoom()
    {
        PlayerSettings.PlayerRoomName = roomName.text;
        PlayerSettings.PlayGame = NetworkManager.GameType.PVP;
        PlayerSettings.JoiningRoom = false;
        SceneManager.LoadScene(1);
    }
    public void CreatePvERoom()
    {
        PlayerSettings.PlayerRoomName = roomName.text;
        PlayerSettings.JoiningRoom = false;
        PlayerSettings.PlayGame = NetworkManager.GameType.PVE;
        SceneManager.LoadScene(1);
    }
}
