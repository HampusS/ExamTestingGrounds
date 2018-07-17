using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBase : WeaponBase
{
    //private void Start()
    //{
    //    boxCollider = GetComponent<BoxCollider>();
    //}

    public override void Execute()
    {
        animator.SetTrigger("Execute");
        animator.speed = 1 - attackRate;
    }

    public override void Running()
    {
        animator.SetBool("Run", true);
    }

    public override void StopRunning()
    {
        animator.SetBool("Run", false);
    }

    public override void Sheath()
    {
        animator.SetBool("Sheath", true);
    }

    public override void Unsheath()
    {
        animator.SetBool("Sheath", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyController temp = other.GetComponent<EnemyController>();
            temp.Damage(damage);
            temp.KnockBack(transform.position);
        }
    }

    public override void EnableHitbox()
    {
        base.EnableHitbox();
    }

    public override void DisableHitbox()
    {
        base.DisableHitbox();
    }

    public override void ResetAnimationSpeed()
    {
        base.ResetAnimationSpeed();
    }
}
