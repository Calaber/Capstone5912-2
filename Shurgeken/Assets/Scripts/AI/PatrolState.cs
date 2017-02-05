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
        RaycastHit hit;
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

        }
    }

    private void Patrol()
    {
        enemy.meshRendererFlag.material.color = Color.green;
        enemy.navMeshAgent.destination = enemy.wayPoints[nextWayPoint].position;
        enemy.navMeshAgent.Resume();
        
        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            nextWayPoint = (nextWayPoint + 1) % enemy.wayPoints.Length;
        }
    }

}
