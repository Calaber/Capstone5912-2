using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightNetworkController : Photon.MonoBehaviour
{

    LightDataController data;


    void Start()
    {
        data = GetComponent<LightDataController>();
        if (photonView.isMine)
        {
            data.local = true;
        }
        else
        {
            StartCoroutine("UpdateData");
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (NetworkManager.networkManager.isMaster())
        {
            data.local = true;
        }
        else
        {
            data.local = false;
        }

        if (stream.isWriting)
        {
            stream.SendNext(data.active);
        }
        else
        {
            data.active = (bool)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void switchLight()
    {
        data.active = !data.active;
    }
}
