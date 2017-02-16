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
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        _volume = 0.2f;


    }
	
	// Update is called once per frame
	void Update () {
        AudioListener.volume = _volume;

    }
}
