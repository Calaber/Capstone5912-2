using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggler : MonoBehaviour
{

    public float light_toggle_range = 3.0f;
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject light = LightManager.nearestLightManager(this.gameObject);
            if (light != null) {
                Vector3 displacement = light.transform.position - this.transform.position;
                if(displacement.magnitude < light_toggle_range)
                    light.GetComponent<LightManager>().toggleLight();
            }
        }
    }
}
