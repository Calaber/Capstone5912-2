using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitConfirmScript : MonoBehaviour {

    public GameObject ExitScreen;
    public Button ExitButton;

	// Use this for initialization
	void Start () {
        ExitButton.onClick.AddListener(OpenExitConfirmation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OpenExitConfirmation()
    {
        ExitScreen.SetActive(true);
    }
}
