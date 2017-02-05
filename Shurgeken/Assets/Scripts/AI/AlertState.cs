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

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            ToChaseState();
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
}
