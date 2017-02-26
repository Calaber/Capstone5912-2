using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState {

    private readonly EnemyStatePattern enemy;

    public AttackState(EnemyStatePattern statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        LookForFlag();
        Look();
        Attack();
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
        enemy.setCurrentState(new ChaseState(enemy));
    }

    public void ToFlagPickUpState()
    {
        enemy.setCurrentState(new FlagPickUp(enemy));
    }

    private void LookForFlag()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Flag"))
        {
            enemy.setChaseTarget(hit.transform);
            ToFlagPickUpState();
        }
    }

    private void Look()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(enemy.transform.position, enemy.enemyViewRadius, enemy.playerLayerMasks);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.transform.position - enemy.transform.position).normalized;
            if (Vector3.Angle(enemy.eyes.transform.forward, dirToTarget) < enemy.enemyViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(enemy.transform.position, target.position);

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
                    if (dstToTarget > enemy.attackDistance)
                    {
                        enemy.setChaseTarget(target.transform);
                        ToChaseState();
                    }
                }
                else
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

    private void Attack()
    {
        enemy.meshRendererFlag.material.color = Color.magenta;
        //The is where the enemy will attack the player, need player contoller code, animation to be added
    }

    public void ToAttackState()
    {
        Debug.Log("Already Attacking Dummy");
    }
}
