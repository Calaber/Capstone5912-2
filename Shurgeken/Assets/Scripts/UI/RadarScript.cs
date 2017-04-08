using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarScript : MonoBehaviour {

    public GameObject[] trackedObjects;
    List<GameObject> radarObjects;
    List<GameObject> borderObjects;

    GameObject Player;
    public GameObject Radar;
    public GameObject JailPrefab;
    public GameObject BlueFlagPrefab;
    GameObject Flag;
    Quaternion rotation = Quaternion.identity;
    float listswitchDistance;
    
    // Use this for initialization
    void Start () {
        rotation.eulerAngles = new Vector3(90, 0, 0);
        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();
        attachJailPrefab();
	}
	
    void findPlayer (){
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null)
        {
            foreach(GameObject looking in players)
            {
                if (looking.GetComponent<PlayerController>().isActiveAndEnabled)
                {
                    Player = looking;
                    foreach (Transform t in Player.transform)
                    {
                        if (t.name == "FriendlyIcon")
                        {
                            t.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    foreach (Transform t in looking.transform)
                    {
                        if (t.name == "PlayerIcon")
                        {
                            t.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
	// Update is called once per frame
	void Update () {
        if (Player == null)
        {
            findPlayer();
        }
        else
        {
            setCameraToPlayer();
        }

        if (Flag == null)
        {
            attachFlagPrefabs();
        }
	}
    void setCameraToPlayer()
    {
        Radar.transform.position = new Vector3 (Player.transform.position.x, Radar.transform.position.y,Player.transform.position.z);
        Radar.transform.eulerAngles = new Vector3(Radar.transform.eulerAngles.x, Player.transform.eulerAngles.y, Radar.transform.eulerAngles.z);

    }
    void attachFlagPrefabs()
    {
        GameObject[] flags;
        flags = GameObject.FindGameObjectsWithTag("Flag");
        if (flags != null)
        {
            foreach (GameObject looking in flags)
            {
                
                GameObject rad = Instantiate(BlueFlagPrefab, new Vector3(looking.transform.position.x, looking.transform.position.y - 30, looking.transform.position.z),rotation);
                rad.transform.parent = looking.transform;
                radarObjects.Add(rad);
                GameObject bord = Instantiate(BlueFlagPrefab, new Vector3(looking.transform.position.x, looking.transform.position.y - 30, looking.transform.position.z), rotation);
                borderObjects.Add(bord);
                bord.transform.parent = looking.transform;
            }
        }
        
    }
    void attachJailPrefab()
    {
        GameObject[] allJails;
        allJails = GameObject.FindGameObjectsWithTag("Jail");
        if (allJails != null)
        {
            foreach (GameObject looking in allJails)
            {
                GameObject rad = Instantiate(JailPrefab, new Vector3(looking.transform.position.x, looking.transform.position.y - 30, looking.transform.position.z),rotation);
                radarObjects.Add(rad);
                
                GameObject bord = Instantiate(BlueFlagPrefab, new Vector3(looking.transform.position.x, looking.transform.position.y - 30, looking.transform.position.z), rotation);
                borderObjects.Add(bord);
            }
        }
    }
}
