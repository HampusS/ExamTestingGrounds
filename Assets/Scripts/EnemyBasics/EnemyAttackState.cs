using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBase {
    public float attackRate = 0.5f;
    public float knockBackStrength = 25;
    public float damage = 25;
    float timer;
    PlayerController playerControl;
    // Use this for initialization
    void Start () {
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
            return true;
        }
        return false;
    }

    void Attack()
    {
        if (!playerControl.isDamaged)
        {
            playerControl.KnockBack((playerControl.transform.position - transform.position).normalized, knockBackStrength);
            playerControl.DamagePlayer(damage);
        }
    }
}
