using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

    public DataController hp;
    public Slider health;
	// Use this for initialization
	void Start () {
        hp = GameObject.FindObjectOfType<DataController>();
        if (hp)
        {
            health.maxValue = hp.max_hp;
            health.value = hp.hp;
        }
    }
	
	// Update is called once per frame
	void Update () {
        hp = GameObject.FindObjectOfType<DataController>();
        if (hp)
        {
            health.maxValue = hp.max_hp;
            health.value = hp.hp;
        }
    }
}
