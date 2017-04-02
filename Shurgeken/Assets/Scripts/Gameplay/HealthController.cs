using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    public delegate void onDamage(int dmg);
    public delegate void onDeath();
    public onDamage onHit;
    public onDeath  onDie;

    DataController data;

    void Start()
    {
        data = GetComponent<DataController>();
    }
    
    public void TakeDamage(int dmg) {
        data.hp -= dmg;
        if (onHit != null) { onHit(dmg); }
        if (data.hp <= 0)
        {
            data.alive = false;
            if (onDie != null) { onDie(); }
        }
    }
}
