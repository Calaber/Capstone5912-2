using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFlag : MonoBehaviour {

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Guard"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
