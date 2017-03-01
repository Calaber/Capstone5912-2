using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadRotaion : MonoBehaviour {


    public Image shur;
    float rotationsPerMinute;
	// Use this for initialization
	void Start () {
        rotationsPerMinute = 50;
	}
	
	// Update is called once per frame
	void Update () {
        

         shur.transform.Rotate(0,0 , 6.0f * rotationsPerMinute * Time.deltaTime);
        
    }
}
