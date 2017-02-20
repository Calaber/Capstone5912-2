using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDataController : MonoBehaviour {

    //Synched data
    public bool active;

    //Not synched
    public bool local = false;

    void Start()
    {
        active = true;
    }
}
