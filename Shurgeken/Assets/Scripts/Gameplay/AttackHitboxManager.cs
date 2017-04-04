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
        int shift = 0;

        for (int i = 0; (i - shift) < colliders.Count; i++)
        {
            obj = colliders[(i - shift)];
            if (obj == null)
            {
                colliders.RemoveAt(i-shift);
                shift++;
                continue;
            }
            try
            {
                while (obj.transform.parent != null) { obj = obj.transform.parent.gameObject; }
                PhotonView target = obj.GetComponent<PhotonView>();
                if (target != null && obj.GetComponent<HealthController>() != null)
                {
                    target.RPC("TakeDamage", PhotonTargets.All, 1);
                }
            }
            catch (MissingReferenceException mre)
            {
                Debug.Log("MissingReferenceException in LightManager nearestLightSource()");
                colliders.RemoveAt(i - shift);
                shift++;
            }
        }


    }


}
