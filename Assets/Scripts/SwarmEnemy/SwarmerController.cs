using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerController : EnemyController
{
    public EnemySpawner spawner;
    
    // Make a ray to player to check line of sight - then add to NavMove enter
    // Make a range check for attack
    // Make an idle behavior

    private void Start()
    {
        states = new List<EnemyBase>();
        states.Add(GetComponent<EnemyIdleState>());
        states.Add(GetComponent<EnemyNavMoveState>());
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
