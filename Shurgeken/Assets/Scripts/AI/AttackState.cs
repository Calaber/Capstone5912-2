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
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        Debug.Log("I'm Already Attacking Dummy");
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

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.transform.position - enemy.transform.position).normalized;
            if (Vector3.Angle(enemy.eyes.transform.forward, dirToTarget) < enemy.enemyViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(enemy.transform.position, target.position);

                if (!Physics.Raycast(enemy.eyes.transform.position, dirToTarget, dstToTarget, enemy.obstacleLayerMasks))
                {
                    if (dstToTarget > enemy.attackDistance)
                    {
                        enemy.chaseTarget = target.transform;
                        ToChaseState();
                    }
                }
                else
                {
                    ToAlertState();
                }
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
