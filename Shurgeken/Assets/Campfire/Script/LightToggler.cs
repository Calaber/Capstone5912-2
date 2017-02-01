using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggler : MonoBehaviour {

    public bool Active;
    public GameObject LightSource;
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        if (LightSource.activeSelf != Active) {
            LightSource.SetActive(Active);
        }
        
	}
}
