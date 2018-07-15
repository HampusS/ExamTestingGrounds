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
        if (controller.InAggroSight())
        {
            Vector3 lookVect = controller.playerControl.transform.position - transform.position;
            lookVect = Vector3.ProjectOnPlane(lookVect, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookVect), Time.deltaTime * 10);
        }
        if (controller.anim != null)
        {
            controller.anim.SetBool("Walking", false);
        }
    }

    public override bool Exit()
    {
        return true;
    }


}
