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
    public static GameObject nearestLightSource(GameObject target, bool active_only)
    {
        GameObject closest = null;
        int shift = 0, index;
        float distance = float.MaxValue;

        for (int i = 0; (i - shift) < lights.Count; i++)
        {
            index = (i - shift);
            if (lights[index] == null)
            {
                lights.RemoveAt(index);
                shift++;
                continue;
            }//updates lights.Count so this iteration should be safe.
            try
            {
                Vector3 displacement = target.transform.position - lights[index].transform.position;
                if (displacement.sqrMagnitude < distance)
                {
                    if (!active_only || lights[index].LightSource.GetActive())
                    {
                        distance = displacement.sqrMagnitude;
                        closest = lights[index].gameObject;
                    }
                }
            }
            catch (MissingReferenceException mre)
            {
                Debug.Log("MissingReferenceException in LightManager nearestLightSource()");
                lights.RemoveAt(index);
                shift++;
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
