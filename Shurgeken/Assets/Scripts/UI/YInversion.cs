using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YInversion : MonoBehaviour {

    public Dropdown Inversion;

    // Use this for initialization
    void Start () {
        setValue();
	}


	void setValue ()
    {
        switch (PlayerSettings.Y_Invert)
        {
            case true:
                Inversion.value = 1;
                break;
            case false:
                Inversion.value = 0;
                break;
            default:
                Inversion.value = 0;
                break;
        }
    }


    void OnEnable()
    {
        setValue();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
