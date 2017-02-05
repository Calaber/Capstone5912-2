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
            Debug.Log("You");
            ToFlagPickUpState();
        }
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.eyes.transform.position, enemy.eyes.transform.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            //Light based view distance cutoff
            GameObject player = hit.transform.gameObject;
            GameObject lighting = LightManager.nearestLightSource(player);

            float player_illumination = lighting.GetComponent<LightManager>().lightIntenstity(player);
            float cutoff_sight_range = enemy.minimumSightRange + (enemy.sightRange - enemy.minimumSightRange) * player_illumination;
           // Debug.Log("Enemy view distance:"cutoff_sight_range + " / 20.0");
            Vector3 displacement = player.transform.position - enemy.transform.position;
            if (displacement.magnitude < cutoff_sight_range)
            {
                enemy.chaseTarget = hit.transform;
                ToChaseState();
            }
            //
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
