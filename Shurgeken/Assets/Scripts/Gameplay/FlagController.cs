using UnityEngine;

public class FlagController : MonoBehaviour {


    public GameObject owner;
    public GameObject previous_owner;
    public int owner_timer;
    public Transform spawnPoint;
    new Rigidbody rigidbody;
    float angle;
    float distance;
    float pass_speed = 10.0f;
    float pass_dampening = 1.1f;
    public bool local = false;


	// Use this for initialization
	void Start () {
        owner = previous_owner = null;
        owner_timer = 0;
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        angle += 0.05f;
        if (owner_timer > 0) { owner_timer--; }
        if (owner)
        {
            this.transform.position = owner.transform.position + new Vector3(1.5f * Mathf.Sin(angle), 2f + (0.3f * Mathf.Sin(2 * angle)), 1.5f * Mathf.Cos(angle));
            this.transform.rotation = Quaternion.Euler(0, angle, 0);
            
        }
        else
        {
            if (rigidbody.velocity.sqrMagnitude > 0.0) { rigidbody.velocity /= pass_dampening; }
        }

    }

    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Player") {
            if (owner == null && (other.gameObject != previous_owner || owner_timer <= 0)) {
                owner = other.gameObject;
                owner.GetComponent<PhotonView>().RPC("EnableAttack", PhotonTargets.All, false);
                //print("pickup");
            }

        }
        if (other.tag == "Goal")
        {
            if (owner)
            {
                owner.GetComponent<PhotonView>().RPC("EnableAttack", PhotonTargets.All, true);
            }
            NetworkManager.networkManager.Destroy(gameObject);
            //print("score");
            GameInitScript.gis.gameMaster.RPC("redTeamScored", PhotonTargets.All);
        }
    }

    public void HandleFlagPass(int id) {
        if (owner != null && id == owner.GetComponent<PhotonView>().viewID)
        {
            owner.GetComponent<PhotonView>().RPC("EnableAttack", PhotonTargets.All, true);
            this.transform.position = owner.transform.position + (owner.transform.TransformDirection(Vector3.forward) * 1.5f) + new Vector3(0, 1.5f, 0);
            rigidbody.velocity = owner.transform.TransformDirection(Vector3.forward) * pass_speed;
            previous_owner = owner;
            owner = null;
            owner_timer = 100;
        }
    }

}
