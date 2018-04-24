using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBase
{
    public Transform OptionalDestination;
    Vector3 safeSpace;

    void Start()
    {
        taskType = EnemyTasks.IDLE;
        if (OptionalDestination != null)
            safeSpace = OptionalDestination.position;
    }

    public override bool Enter()
    {
        if (safeSpace != Vector3.zero && !navMesh.hasPath)
        {
            navMesh.SetDestination(safeSpace);
            return false;
        }
        return true;
    }

    public override void Run()
    {
        
    }

    public override bool Exit()
    {
        return true;
    }


}
