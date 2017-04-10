using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelSettings : MonoBehaviour {

    public Button cancel;
    public MenuLogic menu;
    // Use this for initialization
    void Start()
    {
        cancel.onClick.AddListener(TaskOnClick);
    }


    void TaskOnClick()
    {
        menu.toMainMenu();
    }
}
