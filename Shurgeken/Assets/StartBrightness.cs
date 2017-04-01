using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBrightness : MonoBehaviour {
    public Slider Brightness;
    private PlayerSettings pSet;
    private BrightnessEffect Brightener;
    // Use this for initialization
    void Start () {
        pSet = GameObject.FindObjectOfType<PlayerSettings>();
        Brightness.value = pSet.brightness;
        Brightener = GameObject.FindObjectOfType<BrightnessEffect>();
    }
    void OnEnable()
    {
        if (pSet)
        {
            Brightness.value = pSet.brightness;
        }
    }

    void OnDisable()
    {
        Brightness.value = pSet.brightness;
    }

    // Update is called once per frame
    void Update () {
        Brightener.SetBrightness(Brightness.value);
	}
}
