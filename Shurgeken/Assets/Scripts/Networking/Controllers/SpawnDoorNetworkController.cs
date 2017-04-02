using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDoorNetworkController : Photon.MonoBehaviour {

    public bool open;
    public GameObject door;

    void Start()
    {
        if (photonView.isMine)
        {
            open = false;
            StartCoroutine("OpenDoor");
        }
    }

    IEnumerator OpenDoor()
    {
        while (true)
        {
            door.active = !open;
            yield return null;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(open);
        }
        else
        {
            open = (bool)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void OpenDoorRPC()
    {
        open = true;
    }
}
