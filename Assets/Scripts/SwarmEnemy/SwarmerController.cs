﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwarmState
{
    ERROR,
    MOVE,
    ATTACK,
    IDLE,
}

public class SwarmerController : MonoBehaviour
{
    public SwarmBase behaviour { get; private set; }
    public Transform target;

    public EnemySpawner spawner;
    public SwarmState stateType;
    List<SwarmBase> states;
    int hp = 2;

    private void Start()
    {
        states = new List<SwarmBase>();
        states.Add(GetComponent<AgentMove>());
        behaviour = states[0];
    }

    void Update()
    {
        if (behaviour.Exit())
        {
            foreach (SwarmBase state in states)
            {
                if (state.Enter())
                {
                    behaviour = state;
                    stateType = behaviour.stateType;
                }
            }
        }
        behaviour.Run();

        if(hp <= 0)
        {
            KillMe();
        }
    }

    public void KillMe()
    {
        spawner.amount--;
        Destroy(this);
    }
}
