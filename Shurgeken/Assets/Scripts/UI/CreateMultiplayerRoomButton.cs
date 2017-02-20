using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        NetworkManager.networkManager.createRoom(roomName.text, NetworkManager.GameType.PVP);
        NetworkManager.networkManager.loadLevel("Demo 5");
    }
    public void CreatePvERoom()
    {
        NetworkManager.networkManager.createRoom(roomName.text, NetworkManager.GameType.PVE);
        NetworkManager.networkManager.loadLevel("Combat");
    }
}
