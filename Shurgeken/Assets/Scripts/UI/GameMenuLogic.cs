﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuLogic : MenuLogic {

    public GameObject Options;
    public GameObject Settings;
    public GameObject KeyBinds;
    public GameObject Score;
    private GameObject WinScreen;
    private GameObject LoseScreen;
    public GameObject KeyboardInfo;
    public GameObject InfoReminder;
    private bool _win;
    private bool gameEnd;

    public bool win
    {
        get { return _win; }
        set { _win = value; }
    }
    
    public override void toMainMenu()
    {
        Settings.SetActive(false);
        Options.SetActive(!Options.activeSelf);
        KeyboardInfo.SetActive(false);
        
        if (!Options.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Score.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Score.SetActive(false);
        }
    }

    public override void toSettings()
    {
        Options.SetActive(false);
        Settings.SetActive(true);
    }
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.Escape))&&!Settings.activeSelf)
        {
            toMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.I) && !Options.activeSelf && !Settings.activeSelf)
        {
            InfoReminder.SetActive(false);
            KeyboardInfo.SetActive(!KeyboardInfo.activeSelf);
        }
    }
    void toEnd()
    {
        if (win)
        {
            WinScreen.SetActive(true);
        }else {
            LoseScreen.SetActive(true);
        }

            Options.SetActive(false);
            Settings.SetActive(false);
           
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        
    }

    void Start()
    {
        gameEnd = false;
        _win = false;
    }
}
