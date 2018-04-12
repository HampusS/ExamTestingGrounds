using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMove : SwarmBase
{
    NavMeshAgent navMesh;
    
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        controller = GetComponent<SwarmerController>();
        stateType = SwarmState.MOVE;
    }

    public override bool Enter()
    {

        return false;
    }

    public override void Run()
    {
        navMesh.SetDestination(controller.target.position);
    }

    public override bool Exit()
    {
        if (transform.position == controller.target.position)
            return true;

        return false;
    }

    
}
