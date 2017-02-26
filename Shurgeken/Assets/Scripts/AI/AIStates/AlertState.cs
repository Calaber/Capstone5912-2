using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState {
    private readonly EnemyStatePattern enemy;
    private float searchTimer;

    public AlertState(EnemyStatePattern statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Search();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {
        searchTimer = 0f;
        enemy.setCurrentState(new PatrolState(enemy));
    }

    public void ToAlertState()
    {
        Debug.Log("I'm Already Alert Dummy");
    }

    public void ToChaseState()
    {
        searchTimer = 0f;
        enemy.setCurrentState(new ChaseState(enemy));
    }

    public void ToFlagPickUpState()
    {
        enemy.setCurrentState(new FlagPickUp(enemy));
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

                GameObject lightObject = LightManager.nearestLightSource(target.gameObject);
                if (lightObject != null)
                {
                    Light light = lightObject.GetComponent<LightManager>().LightSource.GetComponent<Light>();

                    float light_factor = 1.0f - (target.position - light.transform.position).magnitude / light.range;//[Adam] TODO: factor in light intensity. Some kind of parabolic function?
                    if (light_factor > 0) { light_cutoff_view_distance += (enemy.enemyViewRadius * 0.5f) * light_factor; }
                }

                if (!Physics.Raycast(enemy.eyes.transform.position, dirToTarget, light_cutoff_view_distance, enemy.obstacleLayerMasks))
                {
                    flagInView = true;
                    enemy.setChaseTarget(target.gameObject);
                    ToFlagPickUpState();
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

                    GameObject lightObject = LightManager.nearestLightSource(target.gameObject);
                    if (lightObject != null)
                    {
                        Light light = lightObject.GetComponent<LightManager>().LightSource.GetComponent<Light>();

                        float light_factor = 1.0f - (target.position - light.transform.position).magnitude / light.range;//[Adam] TODO: factor in light intensity. Some kind of parabolic function?
                        if (light_factor > 0) { light_cutoff_view_distance += (enemy.enemyViewRadius * 0.5f) * light_factor; }
                    }

                    if (!Physics.Raycast(enemy.eyes.transform.position, dirToTarget, light_cutoff_view_distance, enemy.obstacleLayerMasks))
                    {
                        enemy.setChaseTarget(target.gameObject);
                        ToChaseState();
                    }
                }
            }
        }
    }

    private void Search()
    {
        enemy.meshRendererFlag.material.color = Color.yellow;
        enemy.getNavMeshAgent().Stop();
        enemy.transform.Rotate(0, enemy.searchingTurnSpeed * Time.deltaTime, 0);
        searchTimer += Time.deltaTime;

        if (searchTimer >= enemy.searchingDuration)
            ToPatrolState();
    }

    public void ToAttackState()
    {
        Debug.Log("I can't attack if I'm not in range");
    }
}
