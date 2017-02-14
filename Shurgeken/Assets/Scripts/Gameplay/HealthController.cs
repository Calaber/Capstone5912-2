using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    public delegate void onDamage(int dmg);
    public delegate void onDeath();
    public onDamage onHit;
    public onDeath  onDie;
    private int cached_hp;

    DataController data;

    void Start()
    {
        data = GetComponent<DataController>();
        cached_hp = data.hp;
    }

    void Update() {
        if (cached_hp != data.hp) {
            if (cached_hp < data.hp) {
                onHit(cached_hp-data.hp);

                if (data.hp <= 0)
                {
                    data.alive = false;
                    onDie();
                }
            }
            cached_hp = data.hp;
        }
    }


    public void TakeDamage(int dmg) {
        data.hp -= dmg;
        onHit(dmg);
        
        if (data.hp <= 0)
        {
            data.alive = false;
            onDie();
        }   
    }
}
