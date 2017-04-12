using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public GameObject   zoom_point;
    public Vector3      camera_angle;
    public float        default_dist;
    private float spherecast_rad = 0.1f;

	// Use this for initialization
	void Start () {
        camera_angle = Vector3.Normalize(camera_angle);
        this.transform.localPosition = camera_angle * default_dist;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit = new RaycastHit();
        int layerMask = (1 << 10)|1;
        if (Physics.SphereCast(zoom_point.transform.position, spherecast_rad, zoom_point.transform.rotation * camera_angle, out hit,default_dist,layerMask)) {
            this.transform.localPosition = camera_angle * Mathf.Min(default_dist, hit.distance-(default_dist*0.1f));
        }
        else
        {
            this.transform.localPosition = camera_angle * default_dist;

        }
	}
}
