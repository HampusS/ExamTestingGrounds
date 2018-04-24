using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMoveState : EnemyBase
{

    void Start()
    {
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

    }

    public override bool Exit()
    {
        return true;
    }


}
