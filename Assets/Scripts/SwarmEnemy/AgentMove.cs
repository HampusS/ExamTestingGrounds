using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMove : SwarmBase
{
    public Transform target;
    NavMeshAgent navMesh;
    
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        stateType = SwarmState.MOVE;
    }

    public override bool Enter()
    {

        return false;
    }

    public override void Run()
    {
        navMesh.SetDestination(target.position);
    }

    public override bool Exit()
    {
        if (transform.position == target.position)
            return true;

        return false;
    }

    
}
