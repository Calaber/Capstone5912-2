using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {
    


    public int max_hp;
    int hp;


	// Use this for initialization
	void Start () {
        hp = max_hp;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(1);
        }

    }

    void TakeDamage(int dmg) {
        if (hp >= dmg)
        {
            hp -= dmg;
            PlayerController pc = gameObject.GetComponent<PlayerController>();
            if (pc)
            {
                DamageIndicator.DamageFlash();
                if (hp <= 0) {
                    pc.Alive = false;
                }
            }
        }
    }
}
