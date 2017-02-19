using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour {


    public GameObject mainMenu;
    public GameObject settings;
    public GameObject network;
    public GameObject createRoom;

    public PlayerSettings playSet;

    public void toSettings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
        network.SetActive(false);
        createRoom.SetActive(false);
    }
    public void toMainMenu()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
        network.SetActive(false);
        createRoom.SetActive(false);
    }
    public void toNetwork()
    {
        settings.SetActive(false);
        mainMenu.SetActive(false);
        network.SetActive(true);
        createRoom.SetActive(false);
    }

    public void toCreateRoom()
    {
        settings.SetActive(false);
        mainMenu.SetActive(false);
        network.SetActive(false);
        createRoom.SetActive(true);
    }

    public void setScreenRes()
    {

    }
}
