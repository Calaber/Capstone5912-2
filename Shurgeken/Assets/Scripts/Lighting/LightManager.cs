using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour {

    private static List<LightManager> lights;
    public bool Extinguishable = true;
    public GameObject LightSource;

	void Start () {
        if (lights == null) { lights = new List<LightManager>();}
        lights.Add(this);
    }

    void onDestroy() {
        lights.Remove(this);
    }

    //Iterates through all managed lights to find the closest one that's active
    public static GameObject nearestLightSource(GameObject target,bool active_only) {
        GameObject closest = null;
        float distance = float.MaxValue;
        bool null_light = false;
        foreach (LightManager light in lights) {
            try
            {
                Vector3 displacement = target.transform.position - light.transform.position;
                if (displacement.sqrMagnitude < distance)
                {
                    if (!active_only || light.LightSource.GetActive()) {
                        distance = displacement.sqrMagnitude;
                        closest = light.gameObject;
                    }
                }
            } catch (MissingReferenceException mre)
            {
                Debug.Log("MissingReferenceException in LightManager nearestLightSource()");
                
                null_light = true;
            }
            if (null_light) break;
        }
        if (null_light) {
            for (int i = 0; i < lights.Count; i++) {
                if(lights[i]==null)lights.RemoveAt(i);//updates lights.Count so this iteration should be safe.
            }
        }

        return closest;
    }


    //Returns a range from 0 to 1 of light intensity from this light.
    public float lightIntenstity(GameObject target) {
        Light light = LightSource.GetComponent<Light>();
        Vector3 displacement = target.transform.position - light.transform.position;
        if (displacement.magnitude > light.range) return 0;
        //else
        return (light.range - displacement.magnitude)/light.range;
        //TODO MAYBE: scale intensity curve by light.intensity
    }

    //Turns the light on and off
    public void toggleLight() {
        if (Extinguishable) {
            LightSource.SetActive(!LightSource.GetActive());
        }
    }

}
