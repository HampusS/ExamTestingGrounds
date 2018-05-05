﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : EnemyController
{
    public EnemySpawner spawner { get; set; }

    private void Start()
    {
        states = new List<EnemyBase>();
        states.Add(GetComponent<EnemyIdleState>());
        states.Add(GetComponent<EnemyNavMoveState>());
        states.Add(GetComponent<EnemyAttackState>());
        currentState = states[0];
    }

    void Update()
    {
        if (!isAlive())
        {
            spawner.amount--;
        }
        StateMachine();
    }


}
