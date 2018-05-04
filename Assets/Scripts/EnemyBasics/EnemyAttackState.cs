using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBase {
    public float attackRate = 0.5f;
    public float knockBackStrength = 25;
    public float damage = 25;
    float timer;
    float animspeed;

    void Start()
    {
        animspeed = navMesh.speed;
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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(controller.playerControl.transform.position - transform.position), Time.deltaTime * 10);
        timer += Time.deltaTime;
        controller.anim.SetTrigger("Loading");
        //controller.anim.speed = controller.anim.speed / attackRate;
        if (timer >= attackRate)
        {
            timer = 0;
            Attack();
        }
    }

    public override bool Exit()
    {
        if (!controller.InAttackRange())
        {
            controller.anim.SetTrigger("Backdown");
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            navMesh.ResetPath();
            return true;
        }
        return false;
    }

    void Attack()
    {
        if (!controller.playerControl.isDamaged)
        {
            controller.anim.SetTrigger("Hit");
            controller.playerControl.KnockBack((controller.playerControl.transform.position - transform.position).normalized, knockBackStrength);
            controller.playerControl.DamagePlayer(damage);
        }
    }
}
