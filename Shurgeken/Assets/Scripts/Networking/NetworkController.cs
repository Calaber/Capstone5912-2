using UnityEngine;
using System.Collections;

public class NetworkController : Photon.MonoBehaviour
{

    public delegate void Respawn(float time);
    public event Respawn RespawnMe;

    DataController data;
    new Rigidbody rigidbody;
    float smoothing = 10f;


    void Start()
    {
        data = GetComponent<DataController>();
        rigidbody = GetComponent<Rigidbody>();
        if (photonView.isMine)
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<DataController>().local = true;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<HealthController>().enabled = true;
            foreach (Camera cam in GetComponentsInChildren<Camera>()) { cam.enabled = true; }
                
            foreach (AudioListener audio in GetComponentsInChildren<AudioListener>())
                audio.enabled = true;
            transform.Find("Head").gameObject.SetActive(false);
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
            rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, data.velocity, Time.deltaTime * smoothing);
            yield return null;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(data.animation_id);
            stream.SendNext(data.position);
            stream.SendNext(data.rotation);
            stream.SendNext(data.velocity);
            stream.SendNext(data.hp);
            stream.SendNext(data.max_hp);
            stream.SendNext(data.alive);
        }
        else
        {
            data.animation_id = (int)stream.ReceiveNext();//TODO: Fix NullPtr occasionally popping up here. [Adam]
            data.position = (Vector3)stream.ReceiveNext();
            data.rotation = (Quaternion)stream.ReceiveNext();
            data.velocity = (Vector3)stream.ReceiveNext();
            data.hp = (int)stream.ReceiveNext();
            data.max_hp = (int)stream.ReceiveNext();
            data.alive = (bool)stream.ReceiveNext();
        }
    }



}