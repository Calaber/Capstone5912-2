using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugControls : MonoBehaviour {
    public GameObject flag;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.F))
        {
            flag.SetActive(true);
        }
	}
}
