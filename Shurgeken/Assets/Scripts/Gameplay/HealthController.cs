using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    DataController data;
    private int respawn_wait;

    void Start()
    {
        data = GetComponent<DataController>();
        respawn_wait = -1;
    }

    	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(1);
        }
        if (respawn_wait >= 0) {
            if (respawn_wait == 0) {
                PhotonNetwork.Destroy(gameObject);
                //if (RespawnMe != null)
                //    RespawnMe(3f);
            }
            respawn_wait--;
        }
    }

    public void TakeDamage(int dmg) {
        data.hp -= dmg;
        if (data.local) { DamageIndicator.DamageFlash(); }
        
        if (data.hp <= 0)
        {
            data.alive = false;
            respawn_wait = 200;
        }   
    }
}
