using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour {

    private float _volume;
    private bool _fullscreen;
    private int _screenWidth;
    private int _screenHeight;

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
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        _volume = 0.2f;
        _screenHeight = Screen.currentResolution.height;
        _screenWidth = Screen.currentResolution.width;
        print(_screenHeight+"x"+_screenWidth);

    }
	
	// Update is called once per frame
	void Update () {
        AudioListener.volume = _volume;

    }
}
