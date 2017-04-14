using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : IEnemyState
{
    private EnemyStatePattern enemy;
    public DamageState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
        enemy.getNavMeshAgent().Stop();
        enemy.GetComponent<DataController>().SetAnimation(Player_Animation.DAMAGED);
        enemy.enemyDamageFrames = 1.0f;
    }
    public void OnTriggerEnter(Collider other)
    {
    }

    public void ToAlertState()
    {
        enemy.setCurrentState(new AlertState(enemy));
    }

    public void ToAttackState()
    {
        Debug.Log("I'm hit");
    }

    public void ToChaseState()
    {
        Debug.Log("I'm hit");
    }

    public void ToFlagPickUpState()
    {
        Debug.Log("I'm hit");
    }

    public void ToPatrolState()
    {
        Debug.Log("I'm hit");
    }

    public void UpdateState()
    {
        if (enemy.enemyDamageFrames < 0.5f)
        {
            ToAlertState();
        }
        else
        {
            enemy.enemyDamageFrames -= Time.deltaTime;
        }
    }
}
