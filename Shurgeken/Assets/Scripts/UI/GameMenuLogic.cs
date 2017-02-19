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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            toOptions();
        }
    }
}
