using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKill : MonoBehaviour{

    void OnTriggerEnter(Collider c){
        Debug.Log("The tag is: " + c.gameObject.tag);
        if (c.gameObject.tag == "Player") {
            c.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All,999);
        }
        if (c.gameObject.tag.ToLower().Trim() == "RedFlag".ToLower().Trim() || c.gameObject.tag.ToLower().Trim() == "BlueFlag".ToLower().Trim()) {
            c.gameObject.GetComponent<PhotonView>().RPC("ResetFlag", PhotonTargets.All);
        }
    }



}
