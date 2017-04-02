using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNetworkPlay : MonoBehaviour {
    private bool once = false;

    void Awake()
    {
        once = false;
    }
    // Use this for initialization
    void Update()
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        if (!once)
        {
            once = true;
            NetworkManager.networkManager.createRoom("myRoom", NetworkManager.GameType.SINGLE);
            NetworkManager.networkManager.loadLevel("Demo 6");
        }
    }
}
