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
    public ResolutionScript resDrop;
    public Dropdown ScreenStyle;

    public PlayerSettings playSet;

    public void toSettings()
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
    public void toMainMenu()
    {
        resDrop.Selected();
        settings.SetActive(false);
        mainMenu.SetActive(true);
        network.SetActive(false);
        createRoom.SetActive(false);
        
        NetworkManager.networkManager.leaveLobby();
    }
    public void toNetwork()
    {
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

}
