using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class toSettingsScript : MonoBehaviour {

    public Button settings;
    public MenuLogic menu;
	// Use this for initialization
	void Start () {
        settings.onClick.AddListener(TaskOnClick);
    }
	

    void TaskOnClick()
    {
        menu.toSettings();
    }
}
