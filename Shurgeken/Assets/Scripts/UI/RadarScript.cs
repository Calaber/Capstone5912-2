using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarScript : MonoBehaviour {

    List<GameObject> radarObjects;
    List<GameObject> borderObjects;

    GameObject Player;
    public GameObject Radar;


    public GameObject JailPrefab;
    public GameObject BlueFlagPrefab;
    public GameObject BlueBasePrefab;
    public GameObject RedBasePrefab;

    GameObject Flag;
    GameObject helpTransform;
    Quaternion rotation = Quaternion.identity;
    float listswitchDistance;
    
    // Use this for initialization
    void Start () {
        rotation.eulerAngles = new Vector3(90, 0, 0);
        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();
        attachJailPrefab();
        attachBlueBasePrefab();
        attachRedBasePrefab();
        listswitchDistance = 25;
        helpTransform = new GameObject();
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
                    transform.parent = Player.transform;
                    setCameraToPlayer();
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
   
        attachFlagPrefabs();
        
        for(int i =0; i < radarObjects.Count; i++)
        {
            if(Mathf.Sqrt(Mathf.Pow(radarObjects[i].transform.position.x-transform.position.x,2)+Mathf.Pow(radarObjects[i].transform.position.z-transform.position.z,2))+4>listswitchDistance&&Player)
            {
                //switch to border object
                
                helpTransform.transform.position = Player.transform.position;
                helpTransform.transform.LookAt(radarObjects[i].transform);
                float jailAdjust = listswitchDistance;
                if (borderObjects[i].tag != "flagIcon") { jailAdjust += 4; }
                borderObjects[i].transform.position = transform.position + listswitchDistance * helpTransform.transform.forward;
                borderObjects[i].transform.position = new Vector3(borderObjects[i].transform.position.x, -30, borderObjects[i].transform.position.z);
                borderObjects[i].layer = LayerMask.NameToLayer("Radar");
                
                radarObjects[i].layer = LayerMask.NameToLayer("InvisRadar");
                
            }
            else
            {
                //switch to radar objects
                borderObjects[i].layer = LayerMask.NameToLayer("InvisRadar");
                radarObjects[i].layer = LayerMask.NameToLayer("Radar");
               
            }
        }
        playersOffRadar();
        guardOffRadar();

	}

    void playersOffRadar()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null)
        {
            foreach (GameObject looking in players)
            {
                if (Mathf.Sqrt(Mathf.Pow(looking.transform.position.x - transform.position.x, 2) + Mathf.Pow(looking.transform.position.z - transform.position.z, 2))+4 > listswitchDistance)
                {
                    foreach (Transform t in looking.transform)
                    {
                        if (t.name == "FriendlyIcon")
                        {
                            t.gameObject.layer= LayerMask.NameToLayer("InvisRadar");
                        }
                    }
                }
                else
                {
                    if (!looking.Equals(Player))
                    {
                        foreach (Transform t in looking.transform)
                        {
                            if (t.name == "FriendlyIcon")
                            {
                                t.gameObject.layer= LayerMask.NameToLayer("Radar");
                            }
                        }
                    }
                }
            }
        }
    }
    void guardOffRadar()
    {
        GameObject[] guards;
        guards = GameObject.FindGameObjectsWithTag("Guard");
        if (guards != null)
        {
            foreach (GameObject looking in guards)
            {
                if (Mathf.Sqrt(Mathf.Pow(looking.transform.position.x - transform.position.x, 2) + Mathf.Pow(looking.transform.position.z - transform.position.z, 2))+4 > listswitchDistance)
                {
                    foreach (Transform t in looking.transform)
                    {
                        if (t.name == "GuardIcon")
                        {
                            t.gameObject.layer = LayerMask.NameToLayer("InvisRadar");
                        }
                    }
                }
                else
                {
                    foreach (Transform t in looking.transform)
                    {
                        if (t.name == "GuardIcon")
                        {
                            t.gameObject.layer = LayerMask.NameToLayer("Radar");
                        }
                    }
                }
            }
        }
    }
    void setCameraToPlayer()
    {
        Radar.transform.position = new Vector3 (Player.transform.position.x, Player.transform.position.y +40,Player.transform.position.z);
        Radar.transform.eulerAngles = new Vector3(Radar.transform.eulerAngles.x, Player.transform.eulerAngles.y, Radar.transform.eulerAngles.z);

    }
    void attachFlagPrefabs()
    {
        if (Flag == null)
        {
           
            Flag = GameObject.FindGameObjectWithTag("RedFlag");
            if (Flag)
            {
                GameObject rad = Instantiate(BlueFlagPrefab, new Vector3(Flag.transform.position.x, Flag.transform.position.y - 30, Flag.transform.position.z), rotation);
                rad.transform.parent = Flag.transform;
                rad.tag = "flagIcon";
      
                radarObjects.Add(rad);
                GameObject bord = Instantiate(BlueFlagPrefab, new Vector3(Flag.transform.position.x, Flag.transform.position.y - 30, Flag.transform.position.z), rotation);
                bord.tag = "flagIcon";
     
                //bord.transform.parent = Flag.transform;
                borderObjects.Add(bord);
            }
        }else
        {
            foreach(GameObject k in radarObjects)
            {
                if (k.tag == "flagIcon")
                {
                    k.transform.position = new Vector3(Flag.transform.position.x, Flag.transform.position.y - 30, Flag.transform.position.z);
                }
            }
            foreach (GameObject k in borderObjects)
            {
                if (k.tag == "flagIcon")
                {
                    k.transform.position = new Vector3(Flag.transform.position.x, Flag.transform.position.y - 30, Flag.transform.position.z);
                }
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
                GameObject rad = Instantiate(JailPrefab, new Vector3(looking.transform.position.x, -30, looking.transform.position.z),rotation);
                radarObjects.Add(rad);
                rad.tag = "jailIcon";
        
                GameObject bord = Instantiate(JailPrefab, new Vector3(looking.transform.position.x, -30, looking.transform.position.z), rotation);
                borderObjects.Add(bord);
                bord.tag = "jailIcon";
              
            }
        }
    }
    void attachRedBasePrefab()
    {
        GameObject RedBase = GameObject.Find("RedTeamFlag");
        if (RedBase)
        {
            
                GameObject rad = Instantiate(RedBasePrefab, new Vector3(RedBase.transform.position.x, RedBase.transform.position.y - 30, RedBase.transform.position.z), rotation);
                rad.transform.parent = RedBase.transform;
                rad.tag = "redBaseIcon";
      
                radarObjects.Add(rad);
                GameObject bord = Instantiate(RedBasePrefab, new Vector3(RedBase.transform.position.x, RedBase.transform.position.y - 30, RedBase.transform.position.z), rotation);
                bord.tag = "redBaseIcon";
           
            borderObjects.Add(bord);
            
        }
    }
    void attachBlueBasePrefab()
    {
        GameObject BlueBase = GameObject.Find("RedTeamFlag");
        if (BlueBase)
        {

            GameObject rad = Instantiate(BlueBasePrefab, new Vector3(BlueBase.transform.position.x, BlueBase.transform.position.y - 30, BlueBase.transform.position.z), rotation);
            rad.transform.parent = BlueBase.transform;
            rad.tag = "blueBaseIcon";
            radarObjects.Add(rad);
           
            GameObject bord = Instantiate(BlueBasePrefab, new Vector3(BlueBase.transform.position.x, BlueBase.transform.position.y - 30, BlueBase.transform.position.z), rotation);
            bord.tag = "blueBaseIcon";
            
            borderObjects.Add(bord);

        }
    }
}
