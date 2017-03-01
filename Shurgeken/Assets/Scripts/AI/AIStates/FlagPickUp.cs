using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPickUp : IEnemyState
{
    private EnemyStatePattern enemy;

    public FlagPickUp(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
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
        Debug.Log("I refuse to do anything but chase the flag");
    }

    public void ToAlertState()
    {
        enemy.setCurrentState(new AlertState(enemy));
    }

    public void ToChaseState()
    {
        Debug.Log("I refuse to do anything but chase the flag");
    }

    public void ToFlagPickUpState()
    {
        Debug.Log("I'm Already PickingUp the Flag");
    }

    private void Look()
    {
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
                    float flagFromSpawn = Vector3.Distance(target.transform.position, target.GetComponent<FlagController>().spawnPoint.position);
                    bool isFlagNotInSpawn = (flagFromSpawn > 0.5f);
                    bool isOwned = target.gameObject.GetComponent<FlagController>().owner == null;
                    if (isOwned && isFlagNotInSpawn)
                    {
                        flagInView = true;
                        enemy.setChaseTarget(target.gameObject);
                        if (checkDistanceToFlag())
                        {
                            GameInitScript.gis.StartCoroutine("SpawnFlag");
                            NetworkManager.networkManager.Destroy(enemy.getChaseTarget().gameObject);
                            ToAlertState();
                        }
                    }
                }
                else
                {
                    ToAlertState();
                }
            }
        }
        if (!flagInView)
        {
            ToAlertState();
        }
    }

    private void Chase()
    {
        enemy.meshRendererFlag.material.color = Color.blue;
        enemy.getNavMeshAgent().destination = enemy.getChaseTarget().transform.position;
        enemy.getNavMeshAgent().Resume();
    }

    public void ToAttackState()
    {
        Debug.Log("Not gonna attack while I'm chasing the flag");
    }

    private Boolean checkDistanceToFlag()
    {
        bool inReturnDis = false;
        float disToFlag = Vector3.Distance(enemy.transform.position, enemy.getChaseTarget().transform.position);

        if(disToFlag < 0.8f)
        {
            inReturnDis = true;
        }

        return inReturnDis;
    }
}
