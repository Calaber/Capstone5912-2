using UnityEngine;


public class PlayerController : MonoBehaviour 
{

    DataController  data;
    PhotonView      pv;
    new Rigidbody   rigidbody;
    public Camera   FPCamera;
    HealthScript HealthBar;


    bool            midair = false;
    bool            falling = false;
    bool            can_jump = true;
    bool            crouching = false;
    bool            busy = false;
    int             busy_frames = 0;
    public int      respawn_timer;
    float           falling_velocity = -1f;
    float           max_airstrafe_velocity = 10.0f;
    float           jump_speed = 12.0f;
    float           gravity = -9.81f;
    float           cam_pitch = 0;
    float           cam_turn = 0;
    public float    run_speed = 6.0f;
    public float    crouched_speed = 1.0f;
    public float    airstrafe_speed = 5.0f;
    public float    mouse_sensitivity = 1.0f;
    //[HideInInspector]
    public bool can_release_from_jail = false;
    public int time_to_release = 200;
    private int jail_release_frames;


    void Start()
    {
        data = GetComponent<DataController>();
        rigidbody = GetComponent<Rigidbody>();
        pv  = GetComponent<PhotonView>();
        //Set Damage handlers
        HealthController hp = GetComponent<HealthController>();
        hp.onHit = this.OnDamage;
        hp.onDie = this.OnDie;

        Cursor.lockState = CursorLockMode.Locked;
        respawn_timer = -1;
        HealthBar = GameObject.FindObjectOfType<HealthScript>();
        HealthBar._hp = data;
        jail_release_frames = 0;
    }

    void Update() {
        //Handle respawning after death
        if (respawn_timer > 0) { respawn_timer--;
            if (respawn_timer == 0) {
                GameInitScript.gis.StartCoroutine("SpawnPlayerInJail", 0);
                NetworkManager.networkManager.Destroy(gameObject);
            }
        }
        if (can_release_from_jail && Input.GetKey(KeyCode.F))
        {
            jail_release_frames++;
            if (jail_release_frames > time_to_release) {
                GameObject.Find("UI Popup").transform.FindChild("Jailbreak").GetComponent<PopupFadeout>().StartPopup();
                GameInitScript.gis.playerTracker.RPC("respawnAllJail", PhotonTargets.All);
            }
        }
        else {
            jail_release_frames = 0;
        }
    }

    void FixedUpdate()
    {
        UpdateMenu();
        UpdateActions();
        //apply gravity
        rigidbody.AddForce(0, gravity, 0, ForceMode.Acceleration);
        //align camera
        FirstPersonCamera();
        CheckIfGrounded();
        //Attack logic: Stand still and cast a damage ray in the middle of the animation.
        if (busy_frames > 0)
        {
            busy_frames--;
            rigidbody.velocity = Vector3.zero;
            if (busy_frames == 30) {
                CastDamageRay(1.0f, 1);
            }
        }
        else {
            busy = false;
        }
        if (data.alive)
        {
            UpdateJumping();
            if (!midair && !busy) { UpdateMovement(); }
            if (midair) { AirControl(); }
        }

        //check if falling (midair w/negative velocity)
        if (rigidbody.velocity.y < falling_velocity)
        {
            midair = true;
            falling = true;
            can_jump = false;
            data.SetAnimation(Player_Animation.FALLING);
        }
        else { falling = false; }

        //Jitter fix. Remove very small velocities from colliding with uneven terrain.
        if (rigidbody.velocity.magnitude < 0.01f/*some very small number*/) {
            rigidbody.velocity = Vector3.zero;
        }

    }

    void UpdateMovement()
    {
        if (!data.alive) { return; }
        Vector3 velocity = rigidbody.velocity;
        Vector3 motion = WorldspaceInput();
        if (motion.magnitude > 1){motion.Normalize();}
        if (motion.magnitude > 0.05f)
        {
            float inputHorizontal = Input.GetAxisRaw("Horizontal");
            float inputVertical = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(inputHorizontal) > Mathf.Abs(inputVertical))
            {
                if (inputHorizontal > 0) { data.SetAnimation(Player_Animation.RUN_RIGHT); }
                else { data.SetAnimation(Player_Animation.RUN_LEFT); }
            }
            else {
                if (inputVertical > 0) { data.SetAnimation(Player_Animation.RUN_FORWARDS); }
                else { data.SetAnimation(Player_Animation.RUN_BACKWARDS); }
            }
        }
        else { data.SetAnimation(Player_Animation.IDLE); }
        velocity = motion * ((crouching)?crouched_speed:run_speed);
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
    }

    void UpdateJumping()
    {
        if (!busy && can_jump && Input.GetButtonDown("Jump"))
        {
            midair = true;
            rigidbody.velocity += jump_speed * Vector3.up;
            data.SetAnimation(Player_Animation.JUMPING);
        }
        if (midair)
        {
            can_jump = false;
            if (falling)
            {
                data.SetAnimation(Player_Animation.FALLING);
            }
        }
    }

    void UpdateMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void UpdateActions() {
        if (Input.GetMouseButtonDown(0) && data.attachEnabled && !busy && !midair){
            if (busy || midair) return;//Dont perform if we're doing something else.
            data.SetAnimation(Player_Animation.MELEE_1);
            busy = true;
            busy_frames = 40;
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameInitScript.gis.redFlag.GetComponent<PhotonView>().RPC("ThrowFlag", PhotonTargets.All, GetComponent<PhotonView>().viewID);
        }
        if (Input.GetKeyDown(KeyCode.M)&& !busy && data.alive)
        {
            GetComponent<HealthController>().TakeDamage(1);
        }
    }

    Vector3 WorldspaceInput()
    {
        Vector3 input;
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        Vector3 forward = gameObject.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        input = inputHorizontal * right + inputVertical * forward;
        return input;
    }

    void FirstPersonCamera()
    {
        cam_pitch += Input.GetAxis("Mouse Y") * mouse_sensitivity;
        cam_turn += Input.GetAxis("Mouse X") * mouse_sensitivity;

        cam_pitch = Mathf.Clamp(cam_pitch, -70, 90);

        transform.eulerAngles = new Vector3(0, cam_turn, 0);
        FPCamera.gameObject.transform.eulerAngles = new Vector3(-cam_pitch, cam_turn, 0);
    }

    void CheckIfGrounded()
    {
        float distanceToGround;
        float threshold = .45f;
        RaycastHit hit;
        Vector3 offset = new Vector3(0, .4f, 0);
        if (Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
        {
            distanceToGround = hit.distance;
            if (distanceToGround < threshold)
            {
                if (falling) {data.SetAnimation(Player_Animation.LANDING);}
                midair = false;
                falling = false;
                can_jump = true;
            }
        }
    }

    void CastDamageRay(float distance,int damage) {
        Vector3 offset = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast((FPCamera.transform.position + offset),FPCamera.transform.TransformDirection(Vector3.forward), out hit, distance))
        {
            GameObject obj = hit.transform.gameObject;
            while(obj.transform.parent != null) { obj = obj.transform.parent.gameObject; }
            //hp_controller.TakeDamage(damage);
            PhotonView target = obj.GetComponent<PhotonView>();
            if (target!=null && obj.GetComponent<HealthController>() != null) {
                target.RPC("TakeDamage", PhotonTargets.All, 1);
            }
            //print("Bang!");
        }
    }

    void AirControl()
    {
        Vector3 motion = WorldspaceInput();
        if (motion.magnitude < 0.05f) return;
        Vector3 air_movement = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        if ((air_movement+(motion * airstrafe_speed)).magnitude <= max_airstrafe_velocity) {
            rigidbody.velocity += motion*airstrafe_speed;
        }
    }

    public void OnDamage(int dmg) {
        data.SetAnimation(Player_Animation.DAMAGED);
        if (data.local) { DamageIndicator.DamageFlash(); }
        busy = true;
        busy_frames = 30;
    }

    public void OnDie() {
        data.SetAnimation(Player_Animation.DYING);
        if (respawn_timer < 0)
        {
            respawn_timer = 200;
        }
    }


}