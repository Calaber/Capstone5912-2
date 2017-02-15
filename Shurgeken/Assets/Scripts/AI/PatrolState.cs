using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {
    private readonly EnemyStatePattern enemy;
    private int nextWayPoint;

    public PatrolState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
    }

    public void UpdateState()
    {
        LookForFlag();
        Look();
        Patrol();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }

    public void ToPatrolState()
    {
        Debug.Log("I'm Already Patroling Dummy");
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToFlagPickUpState()
    {
        enemy.currentState = enemy.pickUp;
    }

    private void LookForFlag()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Flag"))
        {
            enemy.chaseTarget = hit.transform;
            ToFlagPickUpState();
        }
    }

    private void Look()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(enemy.transform.position, enemy.enemyViewRadius, enemy.playerLayerMasks);

        for(int i=0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.transform.position - enemy.transform.position).normalized;
            if (Vector3.Angle(enemy.eyes.transform.forward, dirToTarget) < enemy.enemyViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(enemy.transform.position, target.position);

                if (!Physics.Raycast(enemy.eyes.transform.position, dirToTarget, dstToTarget, enemy.obstacleLayerMasks))
                {
                    enemy.chaseTarget = target.transform;
                    ToChaseState();
                }
            }
        }
       /* RaycastHit hit; //Old Code Keeping here for time being
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange)&&hit.collider.CompareTag("Player")){
            
            //Light based view distance cutoff
            GameObject player = hit.transform.gameObject;
            GameObject lighting = LightManager.nearestLightSource(player);
            float player_illumination = lighting.GetComponent<LightManager>().lightIntenstity(player);
            float cutoff_sight_range = enemy.minimumSightRange + (enemy.sightRange - enemy.minimumSightRange) * player_illumination;
            // Debug.Log("Enemy view distance:"cutoff_sight_range + " / 20.0");
            Vector3 displacement = player.transform.position - enemy.transform.position;
            if (displacement.magnitude < cutoff_sight_range) {
                enemy.chaseTarget = hit.transform;
                ToChaseState();
            }
            //

        }*/
    }

    private void Patrol()
    {
        enemy.meshRendererFlag.material.color = Color.green;
        enemy.navMeshAgent.destination = enemy.patrolPath.getPath()[nextWayPoint].position;
        enemy.navMeshAgent.Resume();
        
        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            nextWayPoint = (nextWayPoint + 1) % enemy.patrolPath.getPath().Length;
        }
    }

    public void ToAttackState()
    {
        Debug.Log("I can't attack if I'm not in range");
    }
}
