using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour {

    public static PathManager pathMan;
    
    public Transform[] list1;
    public Transform[] list2;
    public Transform[] list3;
    public Transform[] list4;
    // Use this for initialization
    void Start () {
        pathMan = this;
	}

    public Transform[] getPath()
    {
        switch ((int) Random.Range(1, 4)){
            case 1:
                return list1;
            case 2:
                return list2;
            case 3:
                return list3;
            case 4:
                return list4;
            default:
                return new Transform[0];
        }
    }
}
