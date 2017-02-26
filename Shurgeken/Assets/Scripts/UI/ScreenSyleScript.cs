using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSyleScript : MonoBehaviour {

    public Dropdown ScreenStyle;
    public PlayerSettings playSet;

	// Use this for initialization
	void Start () {
        if (playSet.fullscreen)
        {
            ScreenStyle.value = 0;
        }
        else
        {

            ScreenStyle.value = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
