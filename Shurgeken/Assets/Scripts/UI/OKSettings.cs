using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKSettings : MonoBehaviour {

    public PlayerSettings pSet;
    public Dropdown ScreenStyle;
    public Dropdown Res;
    public MenuLogic menu;

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
        print(newRes);
        pSet.screenHeight = newRes[(Screen.resolutions.Length - 1) - Res.value].height;
        pSet.screenWidth =newRes[(Screen.resolutions.Length - 1) - Res.value].width;
    }


    public void okClick()
    {

        setScreenStyle();
        setRes();
        Screen.SetResolution(pSet.screenWidth, pSet.screenHeight, pSet.fullscreen);
        menu.toMainMenu();
    }

    public void applyClick()
    {
        setScreenStyle();
        setRes();
        Screen.SetResolution(pSet.screenWidth, pSet.screenHeight, pSet.fullscreen);
        
    }
}
