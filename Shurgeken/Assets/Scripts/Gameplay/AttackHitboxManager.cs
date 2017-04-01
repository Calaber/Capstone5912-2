using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxManager : MonoBehaviour {
    public List<GameObject> colliders = new List<GameObject>();
 
    void OnTriggerEnter(Collider newcoll)
    {
        if (!colliders.Contains(newcoll.gameObject))
        {
            if(newcoll.gameObject.GetComponent<HealthController>() != null)
            colliders.Add(newcoll.gameObject);
        }
    }

    void OnTriggerExit(Collider oldcoll)
    {
        if (colliders.Contains(oldcoll.gameObject))
        {
            colliders.Remove(oldcoll.gameObject);
        }
    }

    public void DoSwing(int dmg)
    {
        GameObject obj;
        foreach (GameObject g in colliders)
        {
            obj = g;
            while (obj.transform.parent != null) { obj = obj.transform.parent.gameObject; }
            PhotonView target = obj.GetComponent<PhotonView>();
            if (target != null && obj.GetComponent<HealthController>() != null)
            {
                target.RPC("TakeDamage", PhotonTargets.All, 1);
            }
        }   
    }


}
