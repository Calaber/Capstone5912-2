using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiNetworkController : Photon.MonoBehaviour
{

    public delegate void Respawn(float time);
    public event Respawn RespawnMe;

    DataController data;
    NavMeshAgent nav;
    EnemyStatePattern enemy;
    float smoothing = 10f;


    void Start()
    {
        data = GetComponent<DataController>();
        nav = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyStatePattern>();
        if (photonView.isMine)
        {
            GetComponent<DataController>().local = true;
            GetComponent<HealthController>().enabled = true;
            nav.enabled = true;
            enemy.enabled = true;
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
            enemy.enabled = true;
            nav.enabled = true;
        }
        else
        {
            enemy.enabled = false;
            nav.enabled = false;
        }

        if (stream.isWriting)
        {
            //stream.SendNext(data.animation_id);
            stream.SendNext(data.position);
            stream.SendNext(data.rotation);
            stream.SendNext(data.hp);
            stream.SendNext(data.max_hp);
            stream.SendNext(data.alive);
        }
        else
        {
            //data.animation_id = (int)stream.ReceiveNext();//TODO: Fix NullPtr occasionally popping up here. [Adam]
            data.position = (Vector3)stream.ReceiveNext();
            data.rotation = (Quaternion)stream.ReceiveNext();
            data.hp = (int)stream.ReceiveNext();
            data.max_hp = (int)stream.ReceiveNext();
            data.alive = (bool)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        data.hp -= damage;
    }
}
