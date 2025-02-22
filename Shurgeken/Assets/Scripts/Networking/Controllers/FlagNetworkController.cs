﻿using System.Collections;
using UnityEngine;

public class FlagNetworkController : Photon.MonoBehaviour
{

    FlagDataController data;
    FlagController flagController;
    Rigidbody rb;
    float smoothing = 10f;


    void Start()
    {
        data = GetComponent<FlagDataController>();
        flagController = GetComponent<FlagController>();
        rb = GetComponent<Rigidbody>();
        if (photonView.isMine)
        {
            flagController.enabled = true;
            data.local = true;
        }
        if (!NetworkManager.networkManager.isMaster())
        {
            Debug.Log("Start bullshit co-routine");
            StartCoroutine("UpdateData");
        }
    }

    IEnumerator UpdateData()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, data.position, Time.deltaTime * smoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, data.rotation, Time.deltaTime * smoothing);
            rb.velocity = Vector3.Lerp(rb.velocity, data.velocity, Time.deltaTime * smoothing);
            yield return null;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.isWriting)
        {
            stream.SendNext(data.position);
            stream.SendNext(data.rotation);
            stream.SendNext(rb.velocity);
        }
        else
        {
            try
            {
                data.position = (Vector3)stream.ReceiveNext();
                data.rotation = (Quaternion)stream.ReceiveNext();
                data.velocity = (Vector3)stream.ReceiveNext();
            }
            catch (System.NullReferenceException nre)
            {
                Debug.Log("nre on flag, this is prolly what is causing the flag to fuck up.");
                stream.ResetWriteStream();
                OnPhotonSerializeView(stream, info);
            }
            catch (System.InvalidCastException ice)
            {
                Debug.Log("Invalid cast exception, this is because the stream is out of sync, just give up and try again");
                stream.ResetWriteStream();
                OnPhotonSerializeView(stream, info);
            }
            catch (System.IndexOutOfRangeException ioore)
            {
                Debug.Log("Catching invalid index, just wait for next set of packets");
            }
        }
    }

    [PunRPC]
    public void ThrowFlag(int id)
    {
        flagController.HandleFlagPass(id);
    }

    [PunRPC]
    public void ResetFlag()
    {
        GameInitScript.gis.StartCoroutine("SpawnFlag");
        NetworkManager.networkManager.Destroy(this.gameObject);
    }
}
