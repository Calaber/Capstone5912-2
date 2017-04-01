using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightNetworkController : Photon.MonoBehaviour
{

    LightManager lm;


    void Start()
    {
        lm = GetComponent<LightManager>();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            stream.SendNext(lm.LightSource.GetActive());
        }
        else
        {
            lm.LightSource.SetActive((bool)stream.ReceiveNext());
        }
    }

    [PunRPC]
    public void switchLight()
    {
        lm.toggleLight();
    }
}
