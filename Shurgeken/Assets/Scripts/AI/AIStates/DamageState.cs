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
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("I'm hit");
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
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
        ToAlertState();
    }
}
