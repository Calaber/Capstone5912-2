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
        LookForFlag();
        Look();
        Search();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {
        searchTimer = 0f;
        enemy.currentState = enemy.patrolState;
    }

    public void ToAlertState()
    {
        Debug.Log("I'm Already Alert Dummy");
    }

    public void ToChaseState()
    {
        searchTimer = 0f;
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

        for (int i = 0; i < targetsInViewRadius.Length; i++)
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
    }

    private void Search()
    {
        enemy.meshRendererFlag.material.color = Color.yellow;
        enemy.navMeshAgent.Stop();
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
