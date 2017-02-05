using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerMoveMent : MonoBehaviour {
    public float movementSpeed = 5;
    public float mouseSen = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        float fowardSpeed = (Input.GetAxis("Vertical")*movementSpeed);
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        float leftRight = Input.GetAxis("Mouse X")* mouseSen;

        transform.Rotate(0, leftRight, 0);

        Vector3 speed = new Vector3(sideSpeed, 0, fowardSpeed);
        speed = transform.rotation * speed;

        CharacterController cc = GetComponent<CharacterController>();

        cc.SimpleMove(speed);
    }
}
