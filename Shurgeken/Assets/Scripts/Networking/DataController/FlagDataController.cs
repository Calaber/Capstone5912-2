using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagDataController : MonoBehaviour {
    //Synched data
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public string owner;//or however else you can identfy a player.
    public string last_owner;
    public int owner_timer;

    //Not synched
    public bool local = false;

    void Start()
    {
        owner = "";
        last_owner = "";
        owner_timer = 0;
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        velocity = new Vector3();
    }

    void FixedUpdate()
    {
        if (local)
        {   //Write position data to be synched
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation;
            velocity = GetComponent<Rigidbody>().velocity;
            //animation, hp, alive managed by their respective controllers
        }
    }
}
