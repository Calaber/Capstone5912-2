using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePattern : MonoBehaviour {
    public float searchingTurnSpeed = 120f;
    public float searchingDuration = 4f;
    public float sightRange = 20f;
    public float minimumSightRange = 10f;

    //Fov stuff
    public float enemyViewRadius = 12f;
    [Range(0,360)]
    public float enemyViewAngle = 110f;
    public LayerMask playerLayerMasks;
    public LayerMask obstacleLayerMasks;
    public Transform eyes;
    public Vector3 offset = new Vector3(0, .5f, 0);

    //Indicator
    public MeshRenderer meshRendererFlag;
    
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public IEnemyState currentState;

    //States that are gonna be refactored in next timebox or I'm gonna kill myself so much
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public FlagPickUp pickUp;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public AttackState deathState;
    [HideInInspector] public AttackState damageState;

    //Pathing
    public Transform[] wayPoints;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public IPatrolPath patrolPath;

    //Attack
    [HideInInspector] public float attackDistance = 2f;

    private void Awake()
    {
        patrolPath = new TestPath(wayPoints);
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        chaseState = new ChaseState(this);
        pickUp = new FlagPickUp(this);
        attackState = new AttackState(this);

        //Set Damage handlers
        HealthController hp = GetComponent<HealthController>();
        hp.onHit = this.OnDamage;
        hp.onDie = this.OnDie;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 0.5f;
        InvokeRepeating("randomizePath", 180, 180);
    }

    void Start () {
        currentState = patrolState;
    }
	
	void Update ()
    {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other) {
        currentState.OnTriggerEnter(other);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void randomizePath()
    {
        patrolPath.randamizePath();
    }

    private void OnDie()
    {
        currentState = deathState;
    }

    private void OnDamage(int dmg)
    {
        currentState = damageState;
    }
}
