using UnityEngine;


//Manages networked state information for each game object
[System.Serializable]

public enum Player_Animation {
    IDLE=0,
    RUN_FORWARDS=1,
    RUN_BACKWARDS=2,
    RUN_LEFT=3,
    RUN_RIGHT=4,
    CROUCH = 5,
    JUMPING =6,
    FALLING=7,
    LANDING=8,
    DAMAGED=9,
    DYING=10,
    MELEE_1=11
}

public enum Guard_Animation
{
    IDLE = 0,
    RUN_FORWARDS = 1,
    DAMAGED = 2,
    DEATH = 3,
    MELEE_1 = 4
}

public class DataController : MonoBehaviour {
    //Synched data
    public int animation_id;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public int hp = 100;
    public int max_hp = 100;
    public bool alive = true;
    public bool attackEnabled = true;
    public bool isInJail;

    public string team;
    
    //Not synched
    public bool local = false;

    void Start()
    {
        animation_id = 0;
        position = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        velocity = new Vector3();
    }

    void FixedUpdate() {
        if (local)
        {   //Write position data to be synched
            position = gameObject.transform.position;
            rotation = gameObject.transform.rotation;
            if (GetComponent<Rigidbody>() != null)
                velocity = GetComponent<Rigidbody>().velocity;
            //animation, hp, alive managed by their respective controllers
        }
    }

    public void SetAnimation(Player_Animation anim){animation_id = (int)anim;}


}
