using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwarmState
{
    ERROR,
    MOVE,
    ATTACK,
    IDLE,
}

public class SwarmerController : MonoBehaviour {
    public SwarmBase behaviour { get; private set; }
    public EnemySpawner spawner;
    List<SwarmBase> states;
    SwarmState stateType;
    int hp = 2;


    private void Start()
    {
        states = new List<SwarmBase>();
        states.Add(GetComponent<AgentMove>());
    }

    void Update () {
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
    }

    public void KillMe()
    {

        spawner.amount--;
    }
}
