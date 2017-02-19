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
        NetworkManager.networkManager.leaveLobby();
        Application.Quit();
    }

    public void BackToMenuScene()
    {
        NetworkManager.networkManager.leaveLobby();
        SceneManager.LoadScene("MainMenu");
    }
}
