using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToNetwork : MonoBehaviour {

    public Button network;
    public MenuLogic menu;
    // Use this for initialization
    void Start()
    {
        network.onClick.AddListener(TaskOnClick);
    }


    void TaskOnClick()
    {
        menu.toNetwork();
    }
}
