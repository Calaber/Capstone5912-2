using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoIt : MonoBehaviour {

    public Sprite OurLogo;
    public Image Logo;
    public Button generate;
    
	// Use this for initialization
	void Start () {
        generate.onClick.AddListener(addLogo);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void addLogo()
    {
        Logo.sprite = OurLogo;
    }
}
