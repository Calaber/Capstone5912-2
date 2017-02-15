using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterAudioSlide : MonoBehaviour {

    public Slider volumeSlider;

	// Use this for initialization
	void Start () {
        AudioListener.volume = volumeSlider.value;
	}
	
	// Update is called once per frame
	void Update () {
        AudioListener.volume = volumeSlider.value;
    }
}
