using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyBase
{
    float startY;
    float endY;
    float targetY;

    float speed;

    bool moveUp;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<EnemyController>();
        taskType = EnemyTasks.IDLE;
        startY = transform.position.y;
        endY = transform.position.y + 0.7f;
    }

    public override bool Enter()
    {
        if (!controller.InAggroRange() || !controller.InAggroRange() && !controller.InAggroSight())
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            moveUp = true;
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (moveUp)
        {
            if (targetY != endY)
            {
                targetY = endY;
                speed = 5;
            }
            else if (transform.position.y >= endY * 0.8f)
                moveUp = false;
        }
        else
        {
            if (targetY != startY)
            {
                targetY = startY;
                speed = 7;
            }
            else if (transform.position.y <= startY * 1.2f)
                moveUp = true;
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), Time.deltaTime * speed);
    }

    public override bool Exit()
    {
        if (controller.InAggroRange() && controller.InAggroSight())
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            return true;
        }
        return false;
    }


}
