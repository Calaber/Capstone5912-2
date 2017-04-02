using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailbreakEnabler : MonoBehaviour {

    // Use this for initialization

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<PlayerController>().can_release_from_jail = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().can_release_from_jail = false;
        }
    }

}
