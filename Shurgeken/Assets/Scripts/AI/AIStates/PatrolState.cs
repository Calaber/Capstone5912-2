using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {
    private readonly EnemyStatePattern enemy;

    public PatrolState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
        enemy.GetComponent<DataController>().SetAnimation(Player_Animation.RUN_FORWARDS);
    }

    public void UpdateState()
    {
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
        enemy.setCurrentState(new AlertState(enemy));
    }

    public void ToChaseState()
    {
        enemy.setCurrentState(new ChaseState(enemy));
    }

    public void ToFlagPickUpState()
    {
        enemy.setCurrentState(new FlagPickUp(enemy));
    }

    private void LookForFlag()
    {
    }

    private void Look()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(enemy.transform.position, enemy.enemyViewRadius, enemy.playerLayerMasks);
        Collider[] flagsInViewRadius = Physics.OverlapSphere(enemy.transform.position, enemy.enemyViewRadius, enemy.flagLayerMasks);
        bool flagInView = false;

        for(int i=0; i < flagsInViewRadius.Length; i++)
        {
            Transform target = flagsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.transform.position - enemy.transform.position).normalized;
            if (Vector3.Angle(enemy.eyes.transform.forward, dirToTarget) < enemy.enemyViewAngle / 2)
            {
                float light_cutoff_view_distance = enemy.enemyViewRadius * 0.5f;/*[Adam] TODO: constant for how much light cuts vision, for now it's half*/

                GameObject lightObject = LightManager.nearestLightSource(target.gameObject, true);
                if (lightObject != null)
                {
                    Light light = lightObject.GetComponent<LightManager>().LightSource.GetComponent<Light>();

                    float light_factor = 1.0f - (target.position - light.transform.position).magnitude / light.range;//[Adam] TODO: factor in light intensity. Some kind of parabolic function?
                    if (light_factor > 0) { light_cutoff_view_distance += (enemy.enemyViewRadius) * light_factor; }
                }

                if (!Physics.Raycast(enemy.eyes.transform.position, dirToTarget, light_cutoff_view_distance, enemy.obstacleLayerMasks))
                {
                    float flagFromSpawn = Vector3.Distance(target.transform.position, target.GetComponent<FlagController>().spawnPoint.position);
                    bool isFlagNotInSpawn = (flagFromSpawn > 0.5f);
                    bool isOwned = target.gameObject.GetComponent<FlagController>().owner == null;
                    if (isOwned && isFlagNotInSpawn)
                    {
                        flagInView = true;
                        enemy.setChaseTarget(target.gameObject);
                        ToFlagPickUpState();
                    }
                }
            }
        }

        if (!flagInView)
        {
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.transform.position - enemy.transform.position).normalized;
                if (Vector3.Angle(enemy.eyes.transform.forward, dirToTarget) < enemy.enemyViewAngle / 2)
                {
                    float light_cutoff_view_distance = enemy.enemyViewRadius * 0.5f;/*[Adam] TODO: constant for how much light cuts vision, for now it's half*/

                    GameObject lightObject = LightManager.nearestLightSource(target.gameObject, true);
                    if (lightObject != null)
                    {
                        Light light = lightObject.GetComponent<LightManager>().LightSource.GetComponent<Light>();

                        float light_factor = 1.0f - (target.position - light.transform.position).magnitude / light.range;//[Adam] TODO: factor in light intensity. Some kind of parabolic function?
                        if (light_factor > 0) { light_cutoff_view_distance += (enemy.enemyViewRadius) * light_factor; }
                    }

                    if (!Physics.Raycast(enemy.eyes.transform.position, dirToTarget, light_cutoff_view_distance, enemy.obstacleLayerMasks))
                    {
                        if (target.gameObject.GetComponent<DataController>().alive)
                        {
                            enemy.setChaseTarget(target.gameObject);
                            ToChaseState();
                        }
                    }
                }
            }
        }
    }

    private void Patrol()
    {
        enemy.getNavMeshAgent().destination = enemy.getPatrolPath().getCurrentWaypoint().position;
        enemy.getNavMeshAgent().Resume();
        
        if (enemy.getNavMeshAgent().remainingDistance <= enemy.getNavMeshAgent().stoppingDistance && !enemy.getNavMeshAgent().pathPending)
        {
            enemy.getPatrolPath().advanceWaypoint();
        }
    }

    public void ToAttackState()
    {
        Debug.Log("I can't attack if I'm not in range");
    }
}
