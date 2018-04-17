using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMoveState : EnemyBase
{
    NavMeshAgent navMesh;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
        taskType = EnemyTasks.MOVE;
    }

    public override bool Enter()
    {
        if (controller.InAggroRange() && controller.InAggroSight() && !controller.InAttackRange())
        {
            controller.Destination = controller.player.transform.position;
            navMesh.SetDestination(controller.Destination);
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (controller.InAttackRange())
        {
            navMesh.isStopped = true;
            navMesh.ResetPath();
        }
    }

    public override bool Exit()
    {
        return true;
    }


}
