using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagPickUp : IEnemyState
{
    private readonly EnemyStatePattern enemy;

    public FlagPickUp(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
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
        enemy.currentState = enemy.alertState;
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
        RaycastHit hit;
        Vector3 enemyToTarget = (enemy.chaseTarget.position + enemy.offset) - enemy.eyes.transform.position;
        if (Physics.Raycast(enemy.eyes.transform.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Flag"))
        {
            enemy.chaseTarget = hit.transform;
        }
        else
        {
            ToAlertState();
        }

    }

    private void Chase()
    {
        enemy.meshRendererFlag.material.color = Color.blue;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.Resume();
    }

    public void ToAttackState()
    {
        Debug.Log("Not gonna attack while I'm chasing the flag");
    }
}
