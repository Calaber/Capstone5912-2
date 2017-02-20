using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuLogic : MonoBehaviour {

    public GameObject Options;
    public GameObject Settings;
    public GameObject KeyBinds;

    void toOptions()
    {
        Options.SetActive(!Options.activeSelf);
        if (!Options.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            toOptions();
        }
    }
}
