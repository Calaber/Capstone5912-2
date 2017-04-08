using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public GameObject   zoom_point;
    public Vector3      camera_angle;
    public float        default_dist;

	// Use this for initialization
	void Start () {
        camera_angle = Vector3.Normalize(camera_angle);
        this.transform.localPosition = camera_angle * default_dist;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(zoom_point.transform.position, zoom_point.transform.rotation * camera_angle, out hit, default_dist)) {
            this.transform.localPosition = camera_angle * Mathf.Min(default_dist, hit.distance*0.9f);
        }
        else
        {
            this.transform.localPosition = camera_angle * default_dist;

        }
	}
}
