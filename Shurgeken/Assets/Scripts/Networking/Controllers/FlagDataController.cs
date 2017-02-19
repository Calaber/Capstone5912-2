using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagDataController : MonoBehaviour {
    //Synched data
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Rigidbody rb;

    //Not synched
    public bool local = false;
    private FlagController flagController;

    void Start()
    {
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        velocity = new Vector3();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (local)
        {
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation;
            velocity = rb.velocity;
        }
    }
}
