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
        PhotonView pv = GameInitScript.gis.getPlayerPhotonView();
        if (pv != null)
        {
            GameInitScript.gis.playerTracker.RPC("removeFromJail", PhotonTargets.All, pv.viewID);
        }
        StartCoroutine("leaveGameQuit");
    }

    public void BackToMenuScene()
    {
        PhotonView pv = GameInitScript.gis.getPlayerPhotonView();
        if (pv != null)
        {
            GameInitScript.gis.playerTracker.RPC("removeFromJail", PhotonTargets.All, pv.viewID);
        }
        StartCoroutine("leaveGameMenu");
    }

    public IEnumerator leaveGameMenu()
    {
        yield return new WaitForSecondsRealtime(1);
        NetworkManager.networkManager.leaveRoom();
        NetworkManager.networkManager.leaveLobby();
        NetworkManager.networkManager.disconnectServer();

        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator leaveGameQuit()
    {
        yield return new WaitForSecondsRealtime(1);

        NetworkManager.networkManager.leaveRoom();
        NetworkManager.networkManager.leaveLobby();
        NetworkManager.networkManager.disconnectServer();
        Application.Quit();
    }

}
