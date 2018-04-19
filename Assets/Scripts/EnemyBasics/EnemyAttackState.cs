using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBase {


    public float attackRate = 0.5f;
    float timer;


	// Use this for initialization
	void Start () {
        controller = GetComponent<EnemyController>();
        taskType = EnemyTasks.ATTACK;
    }

    public override bool Enter()
    {
        if (controller.InAttackRange())
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            timer = 0;
            return true;
        }
        return false;
    }

    public override void Run()
    {
        timer += Time.deltaTime;
        if(timer >= attackRate)
        {
            timer = 0;
            Attack();
        }
    }

    public override bool Exit()
    {
        if (!controller.InAttackRange())
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            return true;
        }
        return false;
    }

    void Attack()
    {
        PlayerController control = controller.player.GetComponent<PlayerController>();
        //control.JumpAway((control.transform.position - transform.position).normalized, 700, 5);
    }
}
