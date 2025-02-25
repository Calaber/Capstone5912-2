﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState {
    private readonly EnemyStatePattern enemy;

    public ChaseState(EnemyStatePattern statePatternEnemy)
    {
        enemy = statePatternEnemy;
        enemy.GetComponent<DataController>().SetAnimation(Player_Animation.RUN_FORWARDS);
    }

    public void UpdateState()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {
        Debug.Log("The enemy is still around I won't Patrol yet Dummy");

    }

    public void ToAlertState()
    {
        enemy.setCurrentState(new AlertState(enemy));
    }

    public void ToChaseState()
    {
        Debug.Log("I'm Already Chasing Dummy");
    }

    public void ToFlagPickUpState()
    {
        enemy.setCurrentState(new AlertState(enemy));
    }

    private void Look()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(enemy.transform.position, enemy.enemyViewRadius, enemy.playerLayerMasks);
        Collider[] flagsInViewRadius = Physics.OverlapSphere(enemy.transform.position, enemy.enemyViewRadius, enemy.flagLayerMasks);
        bool flagInView = false;

        for (int i = 0; i < flagsInViewRadius.Length; i++)
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

                    float dstToTarget = Vector3.Distance(enemy.transform.position, target.position);
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
                            if (dstToTarget <= enemy.attackDistance)
                            {
                                ToAttackState();
                            }
                        }else
                        {
                            ToAlertState();
                        }
                    }
                    else
                    {
                        ToAlertState();
                    }
                }
            }
        }
    }

    private void Chase()
    {
        if (enemy.getChaseTarget() != null)
        {
            if (enemy.getChaseTarget().GetComponent<DataController>() !=null && enemy.getChaseTarget().GetComponent<DataController>().alive)
            {
                enemy.getNavMeshAgent().destination = enemy.getChaseTarget().transform.position;
                enemy.getNavMeshAgent().Resume();
            }
        }
        else
        {
            ToAlertState();
        }
    }

    public void ToAttackState()
    {
        enemy.setCurrentState(new AttackState(enemy));
    }
}
