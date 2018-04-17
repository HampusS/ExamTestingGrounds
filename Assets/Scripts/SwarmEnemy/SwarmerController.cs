using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerController : EnemyController
{
    public EnemySpawner spawner;

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
        StateMachine();

        if(health <= 0)
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
