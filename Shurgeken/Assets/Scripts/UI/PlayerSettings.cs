using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour {

    private float _volume;
    private bool _fullscreen;
    private int _screenWidth;
    private int _screenHeight;
    private bool _inGame;
    private float _contrast;
    private float _brightness;

    public float volume
    {
        get { return _volume; }
        set { _volume = value; }
    }
    public bool fullscreen
    {
        get { return _fullscreen; }
        set { _fullscreen = value; }
    }

    public int screenWidth
    {
        get { return _screenWidth; }
        set { _screenWidth = value; }
    }
    public int screenHeight
    {
        get { return _screenHeight; }
        set { _screenHeight = value; }
    }

    public bool inGame
    {
        get { return _inGame; }
        set { _inGame = value; }
    }
    public float contrast
    {
        get { return _contrast; }
        set { _contrast = value; }
    }
    public float brightness
    {
        get { return _brightness; }
        set { _brightness = value; }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        _volume = 0.2f;
        _fullscreen = Screen.fullScreen;
        _screenHeight = Screen.currentResolution.height;
        _screenWidth = Screen.currentResolution.width;
        _inGame = false;
        _brightness = 0.5f;
        _contrast = 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
        AudioListener.volume = _volume;

    }
}
