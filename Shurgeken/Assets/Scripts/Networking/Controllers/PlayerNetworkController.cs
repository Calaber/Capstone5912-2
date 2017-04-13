using UnityEngine;
using System.Collections;

public class PlayerNetworkController  : Photon.MonoBehaviour
{

    public delegate void Respawn(float time);
    public event Respawn RespawnMe;

    DataController data;
    HealthController health;
    new Rigidbody rigidbody;
    float smoothing = 10f;


    void Start()
    {
        data = GetComponent<DataController>();
        health = GetComponent<HealthController>();
        rigidbody = GetComponent<Rigidbody>();
        if (photonView.isMine)
        {
            rigidbody.useGravity = true;
            data.local = true;
            data.team = "red";
            GetComponent<LightToggler>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
            health.enabled = true;
            foreach (Camera cam in GetComponentsInChildren<Camera>()) {
                cam.enabled = true;
                cam.gameObject.AddComponent<BrightnessEffect>();
                BrightnessEffect brights = cam.GetComponent<BrightnessEffect>();
                brights.enabled=true;
                cam.gameObject.GetComponent<AVProMovieCaptureFromScene>().enabled = true;


            }

            foreach (AudioListener audio in GetComponentsInChildren<AudioListener>())
                audio.enabled = true;
            //transform.Find("Head").gameObject.SetActive(false);       [Adam]:Keeping head now, switching to 3rd person.
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
            try
            {
                data.animation_id = (int)stream.ReceiveNext();//TODO: Fix NullPtr occasionally popping up here. [Adam]
                data.position = (Vector3)stream.ReceiveNext();
                data.rotation = (Quaternion)stream.ReceiveNext();
                data.velocity = (Vector3)stream.ReceiveNext();
                data.hp = (int)stream.ReceiveNext();
                data.max_hp = (int)stream.ReceiveNext();
                data.alive = (bool)stream.ReceiveNext();
            }
            catch (System.NullReferenceException nre)
            {
                Debug.Log("Null reference exception occured, idgaf");
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    [PunRPC]
    public void EnableAttack (bool able)
    {
        data.attackEnabled = able;
    }

    [PunRPC]
    public void RespawnAtBase(int viewid)
    {
        if (GetComponent<PhotonView>().viewID == viewid && data.isInJail)
        {
            // Try to be safe
            foreach (Camera cam in GetComponentsInChildren<Camera>()) { cam.enabled = false; }
            foreach (AudioListener audio in GetComponentsInChildren<AudioListener>()) { audio.enabled = false; }

            GameInitScript.gis.StartCoroutine("SpawnPlayer", 0);
            NetworkManager.networkManager.Destroy(gameObject);
        }
    }

}

