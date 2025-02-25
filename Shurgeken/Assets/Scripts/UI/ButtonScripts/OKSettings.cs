﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKSettings : MonoBehaviour {

    public PlayerSettings pSet;
    public Dropdown ScreenStyle;
    public Dropdown Res;
    public MenuLogic menu;
    public Slider Brightness;
    public Slider Contrast;
    public Dropdown Inverter;

    void Start()
    {
        pSet = GameObject.FindObjectOfType<PlayerSettings>(); 
    }

    void setScreenStyle()
    {

        if (ScreenStyle.captionText.text == "Fullscreen")
        {
            pSet.fullscreen = true;

        }
        else
        {
            pSet.fullscreen = false;
        }
    }

    void setRes()
    {
        Resolution [] newRes = Screen.resolutions;
        pSet.screenHeight = newRes[(Screen.resolutions.Length - 1) - Res.value].height;
        pSet.screenWidth =newRes[(Screen.resolutions.Length - 1) - Res.value].width;
    }

    void setBrightContrast()
    {
        pSet.brightness = Brightness.value;
        pSet.contrast = Contrast.value;
    }

    void setInvert()
    {
        PlayerController.invert_y_axis = (Inverter.value == 1);
        PlayerSettings.Y_Invert = (Inverter.value == 1);
    }
    public void okClick()
    {

        setScreenStyle();
        setRes();
        Screen.SetResolution(pSet.screenWidth, pSet.screenHeight, pSet.fullscreen);
        setBrightContrast();
        setInvert();
        menu.toMainMenu();
    }

    public void applyClick()
    {
        setScreenStyle();
        setRes();
        setInvert();
        setBrightContrast();
        Screen.SetResolution(pSet.screenWidth, pSet.screenHeight, pSet.fullscreen);
        
    }


}
