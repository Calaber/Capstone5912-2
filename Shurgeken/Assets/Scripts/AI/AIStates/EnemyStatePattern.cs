using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatePattern : MonoBehaviour {
    //Ai Settings
    //Search Settings
    public float searchingTurnSpeed = 120f;
    public float searchingDuration = 4f;

    //Attack Settings
    public float attackDistance = 1f;

    //Fov settings
    public float sightRange = 20f;
    public float minimumSightRange = 10f;
    public float enemyViewRadius = 12f;
    [Range(0,360)]
    public float enemyViewAngle = 110f;
    public LayerMask playerLayerMasks;
    public LayerMask obstacleLayerMasks;
    public LayerMask flagLayerMasks;
    public Transform eyes;
    public Vector3 offset = new Vector3(0, .5f, 0);

    //Indicator
    public MeshRenderer meshRendererFlag;
    
    //
    [HideInInspector] private IEnemyState currentState;
    
    //Pathing
    [HideInInspector] private IPatrolPath patrolPath;
    [HideInInspector] private NavMeshAgent navMeshAgent;
    [HideInInspector] private GameObject chaseTarget;

    private void Awake()
    {
        patrolPath = new TestPath(PathManager.pathMan.getPath());

        //Set Damage handlers
        HealthController hp = GetComponent<HealthController>();
        hp.onHit = this.OnDamage;
        hp.onDie = this.OnDie;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 0.5f;
        InvokeRepeating("randomizePath", 180, 180);
    }

    void Start () {
        currentState = new PatrolState(this);
    }
	
	void Update ()
    {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other) {
        currentState.OnTriggerEnter(other);
    }


    private void randomizePath()
    {
        patrolPath.randamizePath();
    }

    //Death and Damage Calls for hp contoller
    private void OnDie()
    {
        currentState = new DeathState(this);
    }

    private void OnDamage(int dmg)
    {
        currentState = new DamageState(this);
    }

    //Method for conversion
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    //Getters and Setters
    public IEnemyState getCurrentState()
    {
        return currentState;
    }

    public void setCurrentState(IEnemyState state)
    {
        currentState = state;
    }

    public GameObject getChaseTarget()
    {
        return chaseTarget;
    }

    public void setChaseTarget(GameObject target)
    {
        chaseTarget = target;
    }
    
    public NavMeshAgent getNavMeshAgent()
    {
        return navMeshAgent;
    }

    public IPatrolPath getPatrolPath()
    {
        return patrolPath;
    }

    public void setPatrolPath(IPatrolPath path)
    {
        patrolPath= path;
    }
}
