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
    PlayerController playerControl;
    // Use this for initialization
    void Start () {
        animspeed = navMesh.speed;
        taskType = EnemyTasks.ATTACK;
        playerControl = controller.player.GetComponent<PlayerController>();
    }

    public override bool Enter()
    {
        if (controller.InAttackRange())
        {
            navMesh.isStopped = true;
            navMesh.ResetPath();
            timer = 0;
            return true;
        }
        return false;
    }

    public override void Run()
    {
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
            return true;
        }
        return false;
    }

    void Attack()
    {
        controller.anim.SetTrigger("Hit");
        if (!playerControl.isDamaged)
        {
            playerControl.KnockBack((playerControl.transform.position - transform.position).normalized, knockBackStrength);
            playerControl.DamagePlayer(damage);
        }
    }
}
