using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void QuitGame()
    {
        NetworkManager.networkManager.leaveRoom();
        NetworkManager.networkManager.leaveLobby();
        NetworkManager.networkManager.disconnectServer();
        Application.Quit();
    }

    public void BackToMenuScene()
    {
        NetworkManager.networkManager.leaveRoom();
        NetworkManager.networkManager.leaveLobby();
        NetworkManager.networkManager.disconnectServer();
        SceneManager.LoadScene("MainMenu");
    }
}
