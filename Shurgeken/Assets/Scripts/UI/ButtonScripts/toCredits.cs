using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class toCredits : MonoBehaviour {


    public Button Credits;
    public MenuLogic menu;

	// Use this for initialization
	void Start () {
        Credits.onClick.AddListener(cred);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void cred()
    {
        menu.toCredits();
    }
}
