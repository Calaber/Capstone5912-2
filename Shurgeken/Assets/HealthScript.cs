using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

    public DataController hp;
    public Slider health;
	// Use this for initialization
	void Start () {
        health.maxValue = hp.max_hp;
        health.value = hp.max_hp;

	}
	
	// Update is called once per frame
	void Update () {
        health.value = hp.hp;
	}
}
