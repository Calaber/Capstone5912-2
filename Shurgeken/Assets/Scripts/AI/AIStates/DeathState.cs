using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IEnemyState
{
    private EnemyStatePattern enemy;
    private float timer;
    public DeathState(EnemyStatePattern enemyStatePattern)
    {
        enemy = enemyStatePattern;
        enemy.GetComponent<DataController>().SetAnimation(Player_Animation.DYING);
        timer = 0.0f;
        enemy.meshRendererFlag.material.color = Color.black;
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
        timer = timer + Time.deltaTime;
        if(timer >= 10.0f)
        {
            GameInitScript.gis.StartCoroutine("SpawnAI");
            NetworkManager.networkManager.Destroy(enemy.gameObject);
        }
    }
}
