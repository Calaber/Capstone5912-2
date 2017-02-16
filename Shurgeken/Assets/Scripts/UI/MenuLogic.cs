using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour {


    public GameObject mainMenu;
    public GameObject settings;
    public GameObject network;

    public PlayerSettings playSet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void toSettings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
        network.SetActive(false);
    }
    public void toMainMenu()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
        network.SetActive(false);
    }
    public void toNetwork()
    {
        settings.SetActive(false);
        mainMenu.SetActive(false);
        network.SetActive(true);
    }

    public void setScreenRes()
    {

    }
}
