using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterAudioSlide : MonoBehaviour {

    public Slider volumeSlider;
    public PlayerSettings volSettings;
	// Use this for initialization
	void Start () {
        volSettings = GameObject.FindObjectOfType<PlayerSettings>();
        volumeSlider.value = volSettings.volume;
        //volSettings.volume = volumeSlider.value;
	}
	
	// Update is called once per frame
	void Update () {
        volSettings.volume = volumeSlider.value;
    }
}
