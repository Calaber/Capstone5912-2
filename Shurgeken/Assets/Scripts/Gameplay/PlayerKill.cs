using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour{

    void OnTriggerEnter(Collider c){
        if (c.gameObject.tag == "Player") {
            c.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All,999);
        }

    }



}
