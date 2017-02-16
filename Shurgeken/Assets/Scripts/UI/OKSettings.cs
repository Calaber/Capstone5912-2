using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKSettings : MonoBehaviour {

    public Button OK;
    public MenuLogic menu;
    // Use this for initialization
    void Start()
    {
        OK.onClick.AddListener(TaskOnClick);
    }


    void TaskOnClick()
    {
        menu.toMainMenu();
    }
}
