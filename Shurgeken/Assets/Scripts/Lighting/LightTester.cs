using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTester : MonoBehaviour {

    public LightToggler light_to_toggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.E)) { light_to_toggle.Active = !light_to_toggle.Active; }


	}
}
