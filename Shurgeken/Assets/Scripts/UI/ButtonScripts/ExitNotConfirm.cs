using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitNotConfirm : MonoBehaviour {

    public GameObject ExitScreen;
    public Button No;

    // Use this for initialization
    void Start () {
        No.onClick.AddListener(LeaveExitConfirmation);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void LeaveExitConfirmation()
    {
        ExitScreen.SetActive(false);
    }
}
