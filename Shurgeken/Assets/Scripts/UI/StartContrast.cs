using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartContrast : MonoBehaviour {
    public Slider Contrast;
    public BrightnessEffect contraster;
    public PlayerSettings pSet;
    // Use this for initialization
    void Start () {
        pSet = GameObject.FindObjectOfType<PlayerSettings>();
        Contrast.value = pSet.contrast;
        contraster = GameObject.FindObjectOfType<BrightnessEffect>();
    }
	void OnEnable()
    {
        if (pSet!=null)
        {
            Contrast.value = pSet.contrast;
        }
    }
    void OnDisable()
    {
        Contrast.value = pSet.contrast;
    }
	// Update is called once per frame
	void Update () {
        contraster.SetContrast(Contrast.value);
    }
}
