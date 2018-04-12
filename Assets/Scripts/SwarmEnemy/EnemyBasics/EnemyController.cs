﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTasks
{
    ERROR,
    IDLE,
    MOVE,
    ATTACK,
}

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    protected int health = 2;
    [SerializeField]
    protected float aggroRange = 180;

    protected EnemyTasks currentTask;
    protected EnemyBase currentState;
    protected List<EnemyBase> states;

    public Vector3 Destination { get; set; }

    public bool InAggroRange()
    {
        Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * aggroRange, Color.red);
        return (Vector3.Distance(transform.position, player.transform.position) < aggroRange);
    }

    public bool InAggroSight()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit);
        if (hit.transform != null && hit.transform.tag != null)
            return (hit.transform.tag == "Player");
        return false;
    }

    public void UpdateDestination()
    {
        Destination = player.transform.position;
    }

    void Start()
    {
        currentTask = EnemyTasks.ERROR;
    }

    void Update()
    {
        StateMachine();
    }

    protected virtual void StateMachine()
    {
        if (currentState.Exit())
        {
            foreach (EnemyBase state in states)
            {
                if (state.Enter())
                {
                    currentTask = state.taskType;
                    currentState = state;
                }
            }
        }
        //Debug.Log(currentTask);
        currentState.Run();
    }
}
