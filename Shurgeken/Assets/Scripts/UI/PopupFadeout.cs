using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupFadeout : MonoBehaviour {


    public int frames_to_last = 200;
    public int frames_to_fade = 100;
    private int frames_lasted, frames_faded;

    private Color current_color;
    private RawImage popup_image;

	// Use this for initialization
	void Start () {
        frames_lasted = frames_to_last;
        frames_faded = frames_to_fade;
        popup_image = this.gameObject.GetComponent<RawImage>();
        current_color = new Color();
        current_color.a = 0;
        popup_image.color = current_color;
    }
	
	// Update is called once per frame
	void Update () {
        if (frames_lasted < frames_to_last) { frames_lasted++; }
        else if (frames_faded < frames_to_fade) { frames_faded++;
            current_color.a = 1.0f - ((float)frames_faded / frames_to_fade);
            popup_image.color = current_color;
        }
    }

    public void StartPopup() {
        if (frames_lasted < frames_to_last) return;//dont'restart if we're not fading yet.
        //(Adam)TODO: set/unset this.active so update methods aren't always being called. ATM generates linker errors trying it?
        frames_lasted = 0;
        frames_faded = 0;
        current_color = Color.white;
        popup_image.color = current_color;
        AudioManager am = this.GetComponent<AudioManager>();
        if (am != null) {
            am.PlaySound(0);
        }
    }
}
