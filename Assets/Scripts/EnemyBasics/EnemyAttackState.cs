using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBase {
    public float attackRate = 1;
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
            controller.anim.SetTrigger("Loading");
            navMesh.enabled = false;
            timer = 0;
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if(controller.damaged)
        {
            timer = 0;
            controller.damaged = false;
        }
        Vector3 lookat = controller.playerControl.transform.position - transform.position;
        lookat = Vector3.ProjectOnPlane(lookat, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookat), Time.deltaTime * 10);
        timer += Time.deltaTime;
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
            navMesh.enabled = true;
            return true;
        }
        return false;
    }

    void Attack()
    {
        if (!controller.playerControl.isInvulnerable)
        {
            controller.anim.SetTrigger("Hit");
            controller.playerControl.KnockBack((controller.playerControl.transform.position - transform.position).normalized, knockBackStrength);
            controller.playerControl.DamagePlayer(damage);
        }
    }
}
