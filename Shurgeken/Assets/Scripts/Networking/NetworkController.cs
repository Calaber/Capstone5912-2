using UnityEngine;
using System.Collections;

public class NetworkController : Photon.MonoBehaviour
{

    public delegate void Respawn(float time);
    public event Respawn RespawnMe;

    public DataController data;
    float smoothing = 10f;


    void Start()
    {

        if (photonView.isMine)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<HealthController>().enabled = true;
            foreach (Camera cam in GetComponentsInChildren<Camera>())
                cam.enabled = true;
            foreach (AudioListener audio in GetComponentsInChildren<AudioListener>())
                audio.enabled = true;
            transform.Find("Body_01.001").gameObject.layer = 8;
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
        if (stream.isWriting)
        {
            stream.SendNext(data);
        }
        else
        {
            data = (DataController)stream.ReceiveNext();
        }
    }



}