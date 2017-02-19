using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagNetworkController : Photon.MonoBehaviour
{

    public delegate void Respawn(float time);
    public event Respawn RespawnMe;

    FlagDataController data;
    FlagController flagController;
    float smoothing = 10f;


    void Start()
    {
        data = GetComponent<FlagDataController>();
        flagController = GetComponent<FlagController>();
        if (photonView.isMine)
        {
            data.local = true;
        }
        else
        {
            StartCoroutine("UpdateData");
        }
    }

    IEnumerator UpdateData()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, data.position, Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, data.rotation, Time.deltaTime * smoothing);
            yield return null;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (NetworkManager.networkManager.isMaster())
        {
            flagController.enabled = true;
        }
        else
        {
            flagController.enabled = false;
        }
        if (stream.isWriting)
        {
            stream.SendNext(data.position);
            stream.SendNext(data.rotation);
        }
        else
        {
            data.position = (Vector3)stream.ReceiveNext();
            data.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
