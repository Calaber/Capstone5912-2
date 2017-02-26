using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

    private DataController hp;
    public Slider health;

    public DataController _hp
    {
        get { return hp; }
        set { hp = value; }
    }
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
        if (hp)
        {
            health.maxValue = hp.max_hp;
            health.value = hp.hp;
        }
    }
}
