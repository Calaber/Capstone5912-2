using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKSettings : MonoBehaviour {

    public PlayerSettings pSet;
    public Dropdown ScreenStyle;
    public MenuLogic menu;

    
    void setScreenStyle()
    {
        //ScreenStyle.ScreenStyle.value
        if (ScreenStyle.captionText.text == "FullScreen")
        {
            pSet.fullscreen = true;
        }
        else
        {
            pSet.fullscreen = false;
        }
    }


    public void okClick()
    {

        setScreenStyle();

        Screen.SetResolution(Screen.width, Screen.height, pSet.fullscreen);
        menu.toMainMenu();
    }

    public void applyClick()
    {
        setScreenStyle();

        Screen.SetResolution(Screen.width, Screen.height, pSet.fullscreen);
    }
}
