using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toCreateRoom : MonoBehaviour {

    public Button createRoom;
    public MenuLogic menu;
    // Use this for initialization
    void Start () {
	    createRoom.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        menu.toCreateRoom();
    }
}
