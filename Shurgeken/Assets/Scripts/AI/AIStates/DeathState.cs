using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IEnemyState
{
    public DeathState(EnemyStatePattern enemyStatePattern)
    {
    }
    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToAlertState()
    {
        Debug.Log("I'm dead");
    }

    public void ToAttackState()
    {
        Debug.Log("I'm dead");
    }

    public void ToChaseState()
    {
        Debug.Log("I'm dead");
    }

    public void ToFlagPickUpState()
    {
        Debug.Log("I'm dead");
    }

    public void ToPatrolState()
    {
        Debug.Log("I'm dead");
    }

    public void UpdateState()
    {

    }
}
