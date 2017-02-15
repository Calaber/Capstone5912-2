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

    //Gonna be replaced soonish
    public Transform[] wayPoints;

    public Transform eyes;
    public Vector3 offset = new Vector3(0, .5f, 0);
    public MeshRenderer meshRendererFlag;

    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public FlagPickUp pickUp;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public IPatrolPath patrolPath;
    [HideInInspector] public float attackDistance = 2f;

    private void Awake()
    {
        patrolPath = new TestPath(wayPoints);
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        chaseState = new ChaseState(this);
        pickUp = new FlagPickUp(this);
        attackState = new AttackState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 0.5f;
        InvokeRepeating("randomizePath", 180, 180);
    }

    // Use this for initialization
    void Start () {
        currentState = patrolState;
    }
	
	// Update is called once per frame
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
}
