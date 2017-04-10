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
    public GameObject credits;
    public ResolutionScript resDrop;
    public Dropdown ScreenStyle;

    public PlayerSettings playSet;

    public virtual void toSettings()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
        network.SetActive(false);
        createRoom.SetActive(false);
        if (playSet.fullscreen)
        {
            ScreenStyle.value = 0;
        }
        else
        {

            ScreenStyle.value = 1;
        }

    }
    public virtual void toMainMenu()
    {
        resDrop.Selected();
        settings.SetActive(false);
        mainMenu.SetActive(true);
        network.SetActive(false);
        createRoom.SetActive(false);
        credits.SetActive(false);

        NetworkManager.networkManager.disconnectServer();
    }
    public void toNetwork()
    {
        NetworkManager.networkManager.connectServer();
        NetworkManager.networkManager.joinLobby();
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
    public void toCredits()
    {
        settings.SetActive(false);
        mainMenu.SetActive(false);
        network.SetActive(false);
        createRoom.SetActive(false);
        credits.SetActive(true);
    }

}
