using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour {


    public GameObject owner;
    public GameObject previous_owner;
    public int owner_timer;
    new Rigidbody rigidbody;
    float angle;
    float distance;
    float pass_speed = 10.0f;
    float pass_dampening = 1.1f;


	// Use this for initialization
	void Start () {
        owner = previous_owner = null;
        owner_timer = 0;
        rigidbody = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        angle+= 0.05f;
        if(owner_timer > 0) { owner_timer--; }
        if (owner)
        {
            this.transform.position = owner.transform.position + new Vector3(1.5f * Mathf.Sin(angle), 2f+ (0.3f*Mathf.Sin(2*angle)), 1.5f * Mathf.Cos(angle));
            this.transform.rotation = Quaternion.Euler(0, angle, 0);
            if (Input.GetMouseButtonDown(1))
            {
                HandleFlagPass();
            }
        }
        else {
            if (rigidbody.velocity.sqrMagnitude > 0.0) { rigidbody.velocity /= pass_dampening; }
        }

    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Player") {
            if (other.gameObject != previous_owner || owner_timer <= 0) {
                owner = other.gameObject;
                owner.GetComponent<PlayerController>().attack_enabled = false;
                //print("pickup");
            }

        }
        if (other.tag == "Goal") {
            GameObject.Destroy(gameObject);
            if (owner) { owner.GetComponent<PlayerController>().attack_enabled = true; }
            //print("score");
        }
    }

    void HandleFlagPass() {

        owner.GetComponent<PlayerController>().attack_enabled = true;
        this.transform.position = owner.transform.position + (owner.transform.TransformDirection(Vector3.forward) * 1.5f) + new Vector3(0, 1.5f, 0);
        rigidbody.velocity = owner.transform.TransformDirection(Vector3.forward)* pass_speed;
        previous_owner = owner;
        owner = null;
        owner_timer = 100;
    }

}
