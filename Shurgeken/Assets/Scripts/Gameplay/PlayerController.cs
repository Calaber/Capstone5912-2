using UnityEngine;


public class PlayerController : MonoBehaviour 
{

    DataController  data;
    PhotonView      pv;
    new Rigidbody   rigidbody;
    public Camera   FPCamera;
    HealthScript HealthBar;
    public Animator anim;

    public static bool invert_y_axis;
    bool            midair = false;
    bool            falling = false;
    bool            can_jump = true;
    bool            crouching = false;
    public bool     attacking = false;
    public int      attack_frames = 0;
    bool            being_damaged = false;
    int             damage_frames = 0;
    public int      respawn_timer;
    float           falling_velocity = -2f;
    float           max_airstrafe_velocity = 10.0f;
    float           jump_speed = 12.0f;
    float           gravity = -9.81f;
    float           cam_pitch = 0;
    float           cam_turn = 0;
    public float    run_speed = 6.0f;
    public float    crouched_speed = 1.0f;
    public float    airstrafe_speed = 5.0f;
    public float    mouse_sensitivity = 1.0f;
    public AttackHitboxManager attack_hitbox;
    //[HideInInspector]
    public bool can_release_from_jail = false;
    public int time_to_release = 200;
    [SerializeField]
    private bool released;
    private int jail_release_frames;


    void Start()
    {
        data = GetComponent<DataController>();
        rigidbody = GetComponent<Rigidbody>();
        pv  = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        //Set Damage handlers
        HealthController hp = GetComponent<HealthController>();
        hp.onHit = this.OnDamage;
        hp.onDie = this.OnDie;

        Cursor.lockState = CursorLockMode.Locked;
        respawn_timer = -1;
        HealthBar = GameObject.FindObjectOfType<HealthScript>();
        HealthBar._hp = data;
        jail_release_frames = 0;
        released = false;
    }



    private int my_anim_id = 0;
    private int my_anim_priority = 0;
    void Update() {}

    void FixedUpdate()
    {
        UpdateTimers();
        
        //apply gravity
        rigidbody.AddForce(0, gravity, 0, ForceMode.Acceleration);
        //align camera
        FirstPersonCamera();

        if (data.alive)
        {
            CheckIfGrounded();
            UpdateJumping();
            if (!midair) { UpdateMovement(); }
            if (midair) { AirControl(); }
            UpdateActions();
        }

        //check if falling (midair w/negative velocity)
        if (rigidbody.velocity.y < falling_velocity)
        {
            midair = true;
            falling = true;
            can_jump = false;
            SetAnimationWithPriority(Player_Animation.FALLING, 4);
        }
        else { falling = false; }


        if (!data.alive || rigidbody.velocity.magnitude < 0.05f/*some very small number*/) {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        }
        
        if (my_anim_id != -1) {
            if (attack_frames == 20 && my_anim_id != (int)Player_Animation.MELEE_1) {
                Debug.Log("Attack overwritten by:" + my_anim_id);
            }
            data.SetAnimation((Player_Animation)my_anim_id);
            my_anim_id = -1;
            my_anim_priority = 0;
        }

    }

    void UpdateTimers() {
        //Handle respawn timer after death
        if (respawn_timer > 0)
        {
            respawn_timer--;
            if (respawn_timer == 0)
            {
                GameInitScript.gis.playerTracker.RPC("removeFromJail", PhotonTargets.All, this.gameObject.GetComponent<PhotonView>().viewID);
                GameInitScript.gis.StartCoroutine("SpawnPlayerInJail", 0);
                NetworkManager.networkManager.Destroy(gameObject);
            }
        }
        //handle jail release timer
        if (can_release_from_jail && Input.GetKey(KeyCode.F))
        {
            jail_release_frames++;
            if (jail_release_frames > time_to_release && !released)
            {
                released = true;
                GameObject.Find("UI Popup").transform.FindChild("Jailbreak").GetComponent<PopupFadeout>().StartPopup();
                GameInitScript.gis.playerTracker.RPC("respawnAllJail", PhotonTargets.All);
            }
        }
        else {
            jail_release_frames = 0;
            released = false;
        }
        //Handle attack timing
        if (attack_frames > 0)
        {
            attack_frames--;
            if (attack_frames == 10)
            {
                attack_hitbox.DoSwing(1);
            }
            if (attack_frames == 0)
            {
                attacking = false;
            }
        }
        
        //Handle timing for being damaged
        if (damage_frames > 0) { damage_frames--; }
        else { being_damaged = false; }
    }


    void UpdateMovement()
    {
        if (!data.alive) { return; }
        Vector3 velocity = rigidbody.velocity;
        Vector3 motion = WorldspaceInput();
        if (motion.magnitude > 1){motion.Normalize();}
        if (!being_damaged)
        {
            if (motion.magnitude > 0.05f)
            {
                float inputHorizontal = Input.GetAxisRaw("Horizontal");
                float inputVertical = Input.GetAxisRaw("Vertical");
                anim.SetFloat("x_velocity", inputHorizontal);
                anim.SetFloat("y_velocity", inputVertical);
                if (Mathf.Abs(inputHorizontal) > Mathf.Abs(inputVertical))
                {
                    if (inputHorizontal > 0) { SetAnimationWithPriority(Player_Animation.RUN_RIGHT, 7); }
                    else { SetAnimationWithPriority(Player_Animation.RUN_LEFT, 7); }
                }
                else {
                    if (inputVertical > 0) { SetAnimationWithPriority(Player_Animation.RUN_FORWARDS, 7); }
                    else { SetAnimationWithPriority(Player_Animation.RUN_BACKWARDS, 7); }
                }
            }
            else if (data.animation_id != (int)Player_Animation.IDLE) { SetAnimationWithPriority(Player_Animation.IDLE, 1); }
            velocity = motion * ((crouching) ? crouched_speed : run_speed);
        }
        else {
            velocity = Vector3.zero;
        }
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
    }

    void UpdateJumping()
    {
        if (!being_damaged && can_jump && Input.GetButtonDown("Jump"))
        {
            //cancel attack
            attacking = false;
            attack_frames = 0;
            //jump
            midair = true;
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jump_speed, rigidbody.velocity.z);
            SetAnimationWithPriority(Player_Animation.JUMPING, 9);
        }
        if (midair)
        {
            can_jump = false;
            if (falling)
            {
                SetAnimationWithPriority(Player_Animation.LANDING, 3);
            }
        }
    }

    void UpdateActions() {
        if (Input.GetMouseButtonDown(0) && !being_damaged && data.attackEnabled)
        {
            SetAnimationWithPriority(Player_Animation.MELEE_1, 8);
            if (!attacking) {
                attacking = true;
                attack_frames = 20;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameInitScript.gis.redFlag.GetComponent<PhotonView>().RPC("ThrowFlag", PhotonTargets.All, GetComponent<PhotonView>().viewID);
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            GameInitScript.gis.doorController.RPC("OpenDoorRPC", PhotonTargets.All);
        }
        if (Input.GetKeyDown(KeyCode.H))//Hide cursor, temp for recording
        {
            Cursor.visible = !Cursor.visible;
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
        cam_pitch += Input.GetAxis("Mouse Y") * mouse_sensitivity * ((invert_y_axis)? -1:1);
        cam_turn += Input.GetAxis("Mouse X") * mouse_sensitivity;

        cam_pitch = Mathf.Clamp(cam_pitch, -70, 90);

        transform.eulerAngles = new Vector3(0, cam_turn, 0);
        FPCamera.gameObject.transform.parent.eulerAngles = new Vector3(-cam_pitch, cam_turn, 0);
        
    }

    void CheckIfGrounded()
    {
        float distanceToGround;
        float threshold = 0.45f;
        RaycastHit hit;
        Vector3 offset = new Vector3(0, 0.4f, 0);
        if (Physics.Raycast((transform.position + offset), -Vector3.up, out hit, threshold+0.1f))
        {
            distanceToGround = hit.distance;
            if (distanceToGround < threshold)//if we are close to the ground, and are about to hit it.
            {
                if (falling && data.alive) { SetAnimationWithPriority(Player_Animation.LANDING, 1); }
                midair = false;
                falling = false;
                can_jump = true;
            }
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
        SetAnimationWithPriority(Player_Animation.DAMAGED, 9);
        if (data.local) { DamageIndicator.DamageFlash(); }
        being_damaged = true;
        damage_frames = 30;
    }

    public void OnDie() {
        SetAnimationWithPriority(Player_Animation.DYING, 10);
        if (respawn_timer < 0)
        {
            respawn_timer = 200;
        }
    }

    //bugfix: Multiple player aimations can be set on the same frame, with only one actually being sent to the animator.
    public void SetAnimationWithPriority(Player_Animation anim, int priority) {
        if (priority > my_anim_priority) {
            my_anim_priority = priority;
            my_anim_id = (int)anim;
        }
    }

}