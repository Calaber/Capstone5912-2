using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxManager : MonoBehaviour {
    public List<Collider> colliders;
 
    void OnTriggerEnter(Collider newcoll)
    {
        if (!colliders.Contains(newcoll))
        {
            colliders.Add(newcoll);
        }
    }

    void OnTriggerExit(Collider oldcoll)
    {
        if (colliders.Contains(oldcoll))
        {
            colliders.Remove(oldcoll);
        }
    }

    public void DoSwing(int dmg)
    {
        GameObject obj;
        foreach (Collider c in colliders)
        {
            obj = c.gameObject;
            while (obj.transform.parent != null) { obj = obj.transform.parent.gameObject; }
            PhotonView target = obj.GetComponent<PhotonView>();
            if (target != null && obj.GetComponent<HealthController>() != null)
            {
                target.RPC("TakeDamage", PhotonTargets.All, 1);
            }
        }   
    }


}
