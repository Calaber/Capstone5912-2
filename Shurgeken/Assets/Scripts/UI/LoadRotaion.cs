using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadRotaion : MonoBehaviour {


    public Image shur;
    float rotationsPerMinute;

    private bool once = false;
    // Use this for initialization
    void Start () {
        rotationsPerMinute = 80;
	}
	
	// Update is called once per frame
	void Update () {
        

         shur.transform.Rotate(0,0 , -6.0f * rotationsPerMinute * Time.deltaTime);

            StartCoroutine("LoadLevel");




    }

    IEnumerator LoadLevel ()
    {

        if (!once)
        {
            once = true;
            NetworkManager.networkManager.createRoom(PlayerSettings.PlayerRoomName, PlayerSettings.PlayGame);
            NetworkManager.networkManager.loadLevel("Demo 6");
        }
        yield return null; 
    }


}
